using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direccion;
    private CinemachineVirtualCamera cm;
    
    [Header("Estadisticas")]
    public float velocidadDeMovimiento = 10;
    public float fuerzaDeSalto = 5;

    [Header("Colisiones")]
    public Vector2 abajo;
    public float radioDeColision;
    public LayerMask layerPiso;

    [Header("Booleanos")]
    public bool puedeMover = true;
    public bool enSuelo = true;

    public bool haciendoShake = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        Movimiento();
        Agarre();
        
    }
    private IEnumerator AgitarCamara()
    {
        haciendoShake = true;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 5;
        yield return new WaitForSeconds(0.3f);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        haciendoShake = false;
    }
    private IEnumerator AgitarCamara(float tiempo)
    {

        haciendoShake = true;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 5;
        yield return new WaitForSeconds(tiempo);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        haciendoShake = false;
    }
    private void Movimiento()
    {
        //Movimiento definido en dos variables, x, y 
        //x obtiene el valor que se tiene en la tecla que se presione
        //vertical seria similar a x
        //direccion se define dependiendo del eje, y caminar depende de la dirección

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        direccion = new Vector2(x, y);
        Caminar();

        //se llama el metodo cuando se selecciona la tecla espacio
        MejorarSalto();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enSuelo)
            {
                Saltar();
            }
            
        }
    }
    private void Agarre()
    {
        enSuelo = Physics2D.OverlapCircle((Vector2)transform.position + abajo, radioDeColision, layerPiso);

    }
    private void Saltar()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * fuerzaDeSalto;

    }
    private void MejorarSalto()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;

        }
        else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.0f - 1) * Time.deltaTime;
        }
    }
    private void Caminar()
    {
        if (puedeMover)
        {
            rb.velocity = new Vector2(direccion.x * velocidadDeMovimiento, rb.velocity.y);

            if(direccion != Vector2.zero)
            {
                if(direccion.x < 0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }else if(direccion.x > 0 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
        }
        
    }
}
