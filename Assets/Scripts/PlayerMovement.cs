using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private LayerMask shelfLayer;
    [SerializeField] private string inputHorizontal1;
    [SerializeField] private string inputVertical1;
    [SerializeField] private string inputHorizontal2;
    [SerializeField] private string inputVertical2;
    private float speed;
    //Audio
    [SerializeField] private SoundController soundController;

    //raycast
    private float raycastLength = 1f;
    private Vector3 raycastOffset = new Vector3(0, -0.5f, 0);
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private Vector2 curVector;
    private Shelves curShelf;
    private SpriteRenderer curShelfSprite;
    private bool isIdle;
    //handleTask
    private GameObject contactingObject;
    private GameObject handledObject;
    private bool canSpawn;
    private bool isHandling;
    //knifeTask
    private GameObject knife;
    //exactObject
    private Hamburger curHamburger;
    //state
    public enum state
    {
        Idle,
        Down,
        Up,
        Left,
        Right
    }
    public state curState;

    [SerializeField] private List<Transform> objPos;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        SetStartValue();
    }

    void Update()
    {
        GetInput();
    }
    private void FixedUpdate()
    {
        Move();
        CreateFoodRaycast();
        CreateShelfRaycast();
    }
    private void SetStartValue()
    {
        isHandling = false;
        curState = state.Idle;
        isIdle = true;
        speed = 4f;
        canSpawn = true;
    }
    private void UpdateAnimation()
    {
        if (movement == Vector2.zero)
        {
            animator.SetInteger("status", 0);
        }
        else if (movement.x < 0)
        {
            curState = state.Left;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("status", 3);
        }
        else if (movement.x > 0)
        {
            curState = state.Right;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetInteger("status", 3);
        }
        else if (movement.x == 0 && movement.y > 0)
        {
            curState = state.Up;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("status", 1);
        }
        else if (movement.x == 0 && movement.y < 0)
        {
            curState = state.Down;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("status", 2);
        }
    }
    private void GetInput()
    {
        if (gameObject.CompareTag("Player1"))
        {
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                isIdle = false;

                //Vector
                movement.x = Input.GetAxisRaw(inputHorizontal1);
                movement.y = Input.GetAxisRaw(inputVertical1);

                UpdateAnimation();
                HandledObject();


            }
            else if (isIdle == false)
            {
                isIdle = true;
                movement = Vector2.zero;
                UpdateAnimation();
                HandledObject();
            }

            if (Input.GetKeyDown(KeyCode.P) && !isHandling)
            {
                HandleObject();
            }
            else if (Input.GetKeyDown(KeyCode.P) && isHandling && curShelf != null && (curShelf.curType == Shelves.type.free
                || curShelf.curType == Shelves.type.knife || curShelf.curType == Shelves.type.stove || curShelf.curType == Shelves.type.bin))
            {
                UnHandleObject();
            }
            else if (Input.GetKeyDown(KeyCode.P) && isHandling && curShelf != null && (curShelf.curType == Shelves.type.sold))
            {
                soundController.PlaySFX(soundController.put_lift);
                if (curHamburger != null)
                {
                    if (TaskController.instance.CheckOrderDone(curHamburger.GetHamburgerName()))
                    {
                        Destroy(handledObject);
                        BasicUnHandle();
                    }
                }  
                else if (handledObject.name.StartsWith("potatoChips"))
                {
                    if (TaskController.instance.CheckOrderDone(handledObject.name))
                    {
                        Destroy(handledObject);
                        BasicUnHandle();
                    }
                }
                else if (handledObject.name.StartsWith("salad"))
                {
                    if (TaskController.instance.CheckOrderDone(handledObject.name))
                    {
                        Destroy(handledObject);
                        BasicUnHandle();
                    }
                }
                else if(handledObject.name.EndsWith("Done"))
                {
                    if (TaskController.instance.CheckOrderDone(handledObject.name))
                    {
                        Destroy(handledObject);
                        BasicUnHandle();
                    }
                }
                else
                {
                    if (TaskController.instance.CheckOrderDone(handledObject.name))
                    {
                        Destroy(handledObject);
                        BasicUnHandle();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.P) && isHandling && contactingObject != null)
            {
                MergeObject();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                SliceObject();
            }
        }
        else if (gameObject.CompareTag("Player2"))
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                isIdle = false;

                //Vector
                movement.x = Input.GetAxisRaw(inputHorizontal2);
                movement.y = Input.GetAxisRaw(inputVertical2);

                UpdateAnimation();
                HandledObject();
            }
            else if (isIdle == false)
            {
                isIdle = true;
                movement = Vector2.zero;
                UpdateAnimation();
                HandledObject();
            }

            if (Input.GetKeyDown(KeyCode.R) && !isHandling)
            {
                HandleObject();
            }
            else if (Input.GetKeyDown(KeyCode.R) && isHandling && curShelf != null && (curShelf.curType == Shelves.type.free
                || curShelf.curType == Shelves.type.knife || curShelf.curType == Shelves.type.stove || curShelf.curType == Shelves.type.bin))
            {
                UnHandleObject();
            }
            else if (Input.GetKeyDown(KeyCode.R) && isHandling && contactingObject != null)
            {
                MergeObject();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SliceObject();
            }
        }

    }
    private void MergeObject()
    {
        if (handledObject.name.StartsWith("burgerBun"))
        {
            if (handledObject.GetComponent<Hamburger>() != null)
            {
                curHamburger = handledObject.GetComponent<Hamburger>();
            }
            if (contactingObject.name.StartsWith("tomato") && contactingObject.name.EndsWith("Sliced"))
            {
                if (!curHamburger.tomato.activeSelf)
                {
                    //merge
                    curHamburger.tomato.SetActive(true);
                    //destroy
                    Destroy(contactingObject);
                    curShelf.Free();
                }

            }
            else if (contactingObject.name.StartsWith("lettuce") && contactingObject.name.EndsWith("Sliced"))
            {
                if (!curHamburger.lettuce.activeSelf)
                {
                    //merge
                    curHamburger.lettuce.SetActive(true);
                    //destroy
                    Destroy(contactingObject);
                    curShelf.Free();
                }
            }
            else if (contactingObject.name.StartsWith("patty") && contactingObject.name.EndsWith("Done"))
            {
                if (!curHamburger.patty.activeSelf)
                {
                    //merge
                    curHamburger.patty.SetActive(true);
                    //destroy
                    Destroy(contactingObject);
                    curShelf.Free();
                }
            }
            else if (contactingObject.name.StartsWith("cheese") && contactingObject.name.EndsWith("Sliced"))
                    {
                if (!curHamburger.cheese.activeSelf)
                {
                    //merge
                    curHamburger.cheese.SetActive(true);
                    //destroy
                    Destroy(contactingObject);
                    curShelf.Free();
                }
            }

        }
        else if (handledObject.name.StartsWith("tomato") && handledObject.name.EndsWith("Sliced"))
        {
            if (contactingObject.name.StartsWith("burgerBun"))
            {
                if (contactingObject.GetComponent<Hamburger>() != null)
                {
                    curHamburger = contactingObject.GetComponent<Hamburger>();
                    if (!curHamburger.tomato.activeSelf)
                    {
                        //merge
                        curHamburger.tomato.SetActive(true);
                        //destroy
                        Destroy(handledObject);
                        BasicUnHandle();
                    }
                }
            }
        }
        else if (handledObject.name.StartsWith("cheese") && handledObject.name.EndsWith("Sliced"))
        {
            if (contactingObject.name.StartsWith("burgerBun"))
            {
                if (contactingObject.GetComponent<Hamburger>() != null)
                {
                    curHamburger = contactingObject.GetComponent<Hamburger>();
                    if (!curHamburger.cheese.activeSelf)
                    {
                        //merge
                        curHamburger.cheese.SetActive(true);
                        //destroy
                        Destroy(handledObject);
                        BasicUnHandle();
                    }
                }
            }
        }
        else if (handledObject.name.StartsWith("lettuce") && handledObject.name.EndsWith("Sliced"))
        {
            if (contactingObject.name.StartsWith("burgerBun"))
            {
                if (contactingObject.GetComponent<Hamburger>() != null)
                {
                    curHamburger = contactingObject.GetComponent<Hamburger>();
                    if (!curHamburger.lettuce.activeSelf)
                    {
                        //merge
                        curHamburger.lettuce.SetActive(true);
                        //destroy
                        Destroy(handledObject);
                        BasicUnHandle();
                    }
                }
            }
        }
        else if (handledObject.name.StartsWith("patty") && handledObject.name.EndsWith("Done"))
        {
            if (contactingObject.name.StartsWith("burgerBun"))
            {
                if (contactingObject.GetComponent<Hamburger>() != null)
                {
                    curHamburger = contactingObject.GetComponent<Hamburger>();
                    if (!curHamburger.patty.activeSelf)
                    {
                        //merge
                        curHamburger.patty.SetActive(true);
                        //destroy
                        Destroy(handledObject);
                        BasicUnHandle();
                    }
                }
            }
            else if (contactingObject.name.StartsWith("rosemary"))
            {
                Destroy(handledObject);
                Destroy(contactingObject);
                BasicUnHandle();
                curShelf.Free();
                Level2Food.instance.MakeBeefsteak(curShelf.transform);
                curShelf.CheckShelfType();
            }
        }
        else if (handledObject.name.StartsWith("rosemary"))
        {
            if (contactingObject.name.StartsWith("patty") && contactingObject.name.EndsWith("Done"))
            {
                Destroy(handledObject);
                Destroy(contactingObject);
                BasicUnHandle();
                curShelf.Free();
                Level2Food.instance.MakeBeefsteak(curShelf.transform);
                curShelf.CheckShelfType();
            }
        }
        else if (handledObject.name.StartsWith("cucumber") && handledObject.name.EndsWith("Sliced"))
        {
            if (contactingObject.name.StartsWith("lettuce") && contactingObject.name.EndsWith("Sliced"))
            {
                if(Level2Food.instance != null)
                {
                    Destroy(handledObject);
                    Destroy(contactingObject);
                    BasicUnHandle();
                    curShelf.Free();
                    Level2Food.instance.MakeSalad(curShelf.transform);
                    curShelf.CheckShelfType();
                }
            }
        }
        if (handledObject != null)
        {
            if (handledObject.name.StartsWith("lettuce") && handledObject.name.EndsWith("Sliced"))
            {
                if (contactingObject.name.StartsWith("cucumber") && contactingObject.name.EndsWith("Sliced"))
                {
                    if (Level2Food.instance != null)
                    {
                        Destroy(handledObject);
                        Destroy(contactingObject);
                        BasicUnHandle();
                        curShelf.Free();
                        Level2Food.instance.MakeSalad(curShelf.transform);
                        curShelf.CheckShelfType();
                    }
                }
            }
        }
        soundController.PlaySFX(soundController.put_lift);
    }

    private void BasicUnHandle()
    {
        UpdateAnimation();
        animator.SetBool("isHandling", false);
        isHandling = false;
        handledObject = null;
    }
    //Pick up
    private void HandleObject()
    {
        if (contactingObject != null)
        {
            soundController.PlaySFX(soundController.put_lift);
            CloneIngredients();
            animator.SetBool("isHandling", true);
            isHandling = true;
            handledObject = contactingObject;

            handledObject.transform.SetParent(transform);
            handledObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
            //check healthbar slice object
            if (curShelf.curType == Shelves.type.knifeAndFood)
            {
                Sliceable sliceAble = handledObject.GetComponent<Sliceable>();
                if (sliceAble != null)
                {
                    sliceAble.TurnOffHealthBar();
                }
            }

            //check processbar cook object
            if (curShelf.curType == Shelves.type.stove)
            {
                Cookable cookable = handledObject.GetComponent<Cookable>();
                if (cookable != null)
                {
                    cookable.transform.localScale *= 1.5f;
                    cookable.TurnOffProcessBar();
                    cookable.TurnOffProcessBar2();
                }
            }

            curShelf.CheckShelfType();
            handledObject.GetComponent<BoxCollider2D>().enabled = false;
            HandledObject();
        }
    }
    private void UnHandleObject()
    {
        soundController.PlaySFX(soundController.put_lift);
        handledObject.transform.position = curShelf.transform.position;
        handledObject.transform.SetParent(curShelf.transform);
        curShelf.CheckShelfType();
        handledObject.GetComponent<BoxCollider2D>().enabled = true;
        handledObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        //check healthbar slice object
        if (curShelf.curType == Shelves.type.knifeAndFood)
        {
            Sliceable sliceAble = handledObject.GetComponent<Sliceable>();
            if (sliceAble != null)
            {
                sliceAble.TurnOnHealthBar();
            }
        }

        //check processbar cook object
        if (curShelf.curType == Shelves.type.stove)
        {
            Cookable cookable = handledObject.GetComponent<Cookable>();
            if (cookable != null)
            {
                cookable.transform.localScale /= 1.5f;
                cookable.Cook();
            }
        }
        //Check bin
        if (curShelf.curType == Shelves.type.bin)
        {
            GameObject.Destroy(handledObject);
        }
        BasicUnHandle();
    }
    private void SliceObject()
    {
        if (curShelf != null && (curShelf.curType == Shelves.type.knife || curShelf.curType == Shelves.type.knifeAndFood))
        {
            soundController.PlaySFX(soundController.cut);
            knife = curShelf.transform.GetChild(0).GetChild(0).gameObject;
            if (knife != null)
            {
                knife.GetComponent<Animator>().SetTrigger("slice");
            }
        }
        //decrease health
        if (contactingObject != null)
        {
            Sliceable sliceable = contactingObject.GetComponent<Sliceable>();
            if (sliceable != null)
            {
                sliceable.Slice();
            }
        }
    }
    private void Move()
    {
        rb.MovePosition(rb.position + movement * Time.deltaTime * speed);
    }


    //Ingredients handle
    private void CloneIngredients()
    {
        if (contactingObject.GetComponent<Ingredients>() != null && contactingObject.GetComponent<Ingredients>().isOriginal)
        {
            GameObject newObj = GameObject.Instantiate(contactingObject);
            newObj.transform.position = contactingObject.transform.position;
            newObj.transform.localScale = contactingObject.transform.localScale;
            newObj.transform.SetParent(curShelf.transform, false);
            contactingObject.GetComponent<Ingredients>().Clone();
        }
    }
    private void HandledObject()
    {
        if (handledObject != null)


            switch (curState)
            {
                case state.Up:
                    handledObject.transform.position = objPos[0].position;
                    handledObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    break;
                case state.Down:
                    handledObject.transform.position = objPos[1].position;
                    handledObject.GetComponent<SpriteRenderer>().sortingOrder = 11;
                    break;
                case state.Left:
                    handledObject.transform.position = objPos[2].position;
                    handledObject.GetComponent<SpriteRenderer>().sortingOrder = 11;
                    break;
                case state.Right:
                    handledObject.transform.position = objPos[3].position;
                    handledObject.GetComponent<SpriteRenderer>().sortingOrder = 11;
                    break;
                case state.Idle:

                    break;
            }
    }
    private void CreateFoodRaycast()
    {
        RaycastHit2D hit;
        hit = new RaycastHit2D();
        if (curState == state.Down)
        {
            hit = Physics2D.Raycast(transform.position + raycastOffset, Vector2.down, raycastLength * 1.5f, foodLayer);
            curVector = Vector2.down;
        }
        else if (curState == state.Up)
        {
            hit = Physics2D.Raycast(transform.position + raycastOffset, Vector2.up, raycastLength, foodLayer);
            curVector = Vector2.up;
        }
        else if (curState == state.Left)
        {
            hit = Physics2D.Raycast(transform.position + raycastOffset, Vector2.left, raycastLength, foodLayer);
            curVector = Vector2.left;
        }
        else if (curState == state.Right)
        {
            hit = Physics2D.Raycast(transform.position + raycastOffset, Vector2.right, raycastLength, foodLayer);
            curVector = Vector2.right;
        }
        else if (curState == state.Idle)
        {
            hit = Physics2D.Raycast(transform.position + raycastOffset, curVector, raycastLength, foodLayer);
        }

        if (hit.collider != null && curShelf != null)
        {
            contactingObject = hit.collider.gameObject;
        }
        else
        {
            contactingObject = null;
        }
    }
    private void CreateShelfRaycast()
    {
        RaycastHit2D hit;
        hit = new RaycastHit2D();
        if (curState == state.Down)
        {
            hit = Physics2D.Raycast(transform.position + raycastOffset, Vector2.down, raycastLength * 1.5f, shelfLayer);
            curVector = Vector2.down;
        }
        else if (curState == state.Up)
        {
            hit = Physics2D.Raycast(transform.position + raycastOffset, Vector2.up, raycastLength, shelfLayer);
            curVector = Vector2.up;
        }
        else if (curState == state.Left)
        {
            hit = Physics2D.Raycast(transform.position + raycastOffset, Vector2.left, raycastLength, shelfLayer);
            curVector = Vector2.left;
        }
        else if (curState == state.Right)
        {
            hit = Physics2D.Raycast(transform.position + raycastOffset, Vector2.right, raycastLength, shelfLayer);
            curVector = Vector2.right;
        }
        else if (curState == state.Idle)
        {
            hit = Physics2D.Raycast(transform.position + raycastOffset, curVector, raycastLength, shelfLayer);
        }

        if (hit.collider != null)
        {
            curShelf = hit.collider.gameObject.GetComponent<Shelves>();
            curShelfSprite = curShelf.gameObject.GetComponent<SpriteRenderer>();
            curShelfSprite.enabled = true;
        }
        else
        {
            if (curShelf != null)
            {
                curShelfSprite.enabled = false;
                curShelfSprite = null;
                curShelf = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (curState == state.Down)
        {
            Gizmos.color = Color.red;

            Vector3 origin = transform.position + raycastOffset;
            Vector3 rayEnd = origin + (Vector3)Vector2.down * raycastLength*1.5f;

            Gizmos.DrawLine(origin, rayEnd);

            Gizmos.DrawSphere(rayEnd, 0.1f);
        }
        else if (curState == state.Up)
        {
            Gizmos.color = Color.red;

            Vector3 origin = transform.position + raycastOffset;
            Vector3 rayEnd = origin + (Vector3)Vector2.up * raycastLength;

            Gizmos.DrawLine(origin, rayEnd);

            Gizmos.DrawSphere(rayEnd, 0.1f);
        }
        else if (curState == state.Left)
        {
            Gizmos.color = Color.red;

            Vector3 origin = transform.position + raycastOffset;
            Vector3 rayEnd = origin + (Vector3)Vector2.left * raycastLength;

            Gizmos.DrawLine(origin, rayEnd);

            Gizmos.DrawSphere(rayEnd, 0.1f);
        }
        else if (curState == state.Right)
        {
            Gizmos.color = Color.red;

            Vector3 origin = transform.position + raycastOffset;
            Vector3 rayEnd = origin + (Vector3)Vector2.right * raycastLength;

            Gizmos.DrawLine(origin, rayEnd);

            Gizmos.DrawSphere(rayEnd, 0.1f);
        }
    }
}
