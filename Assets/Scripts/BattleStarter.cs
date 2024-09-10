using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour {

    public BAttleType[] potentialBattles; // Lista de posibles batallas que pueden iniciarse

    public bool activateOnEnter, activateOnStay, activateOnExit; // Flags para determinar cuándo iniciar la batalla (al entrar, permanecer o salir de la zona)

    private bool inArea; // Indica si el jugador está dentro del área de activación
    public float timeBetweenBattles = 10f; // Tiempo entre batallas
    private float betweenBattleCounter; // Contador para el tiempo entre batallas

    public bool deactivateAfterStarting; // Si es verdadero, desactiva el activador después de iniciar la batalla

    public bool cannotFlee; // Determina si es posible huir de la batalla

    public bool shouldCompleteQuest; // Indica si completar una misión después de la batalla
    public string QuestToComplete; // Nombre de la misión a completar

    // Inicialización
	void Start () {
        // Establece el contador entre batallas con un rango aleatorio
        betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
	}
	
	// Se llama una vez por frame
	void Update () {
		// Si el jugador está en el área y puede moverse
		if(inArea && PlayerController.instance.canMove)
        {
            // Si el jugador está en movimiento (usando teclado o joystick)
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || PlayerController.instance.joystick.Horizontal != 0 || PlayerController.instance.joystick.Vertical != 0)
            {
                betweenBattleCounter -= Time.deltaTime; // Disminuye el contador entre batallas
            }

            // Si el contador llega a 0, inicia una batalla
            if(betweenBattleCounter <= 0)
            {
                // Reinicia el contador entre batallas con un nuevo valor aleatorio
                betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);

                // Inicia la batalla
                StartCoroutine(StartBattleCo());
            }
        }
	}

    // Se llama cuando el jugador entra en el área de activación
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player") // Verifica si el objeto que entra es el jugador
        {
            if (activateOnEnter) // Si la batalla debe activarse al entrar
            {
                StartCoroutine(StartBattleCo()); // Inicia la batalla
            }
            else
            {
                inArea = true; // Marca que el jugador está en el área
            }
        }
    }

    // Se llama cuando el jugador sale del área de activación
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") // Verifica si el objeto que sale es el jugador
        {
            if (activateOnExit) // Si la batalla debe activarse al salir
            {
                StartCoroutine(StartBattleCo()); // Inicia la batalla
            }
            else
            {
                inArea = false; // Marca que el jugador ha salido del área
            }
        }
    }

    // Corrutina que maneja el inicio de la batalla
    public IEnumerator StartBattleCo()
    {
        UIFade.instance.FadeToBlack(); // Inicia la transición a pantalla negra
        GameManager.instance.battleActive = true; // Marca que la batalla está activa

        int selectedBattle = Random.Range(0, potentialBattles.Length); // Selecciona una batalla aleatoria de las posibles

        // Establece las recompensas para la batalla seleccionada
        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardXP = potentialBattles[selectedBattle].rewardXP;

        yield return new WaitForSeconds(1.5f); // Espera un breve momento antes de iniciar la batalla

        // Inicia la batalla con los enemigos seleccionados y la opción de huir
        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies, cannotFlee);
        UIFade.instance.FadeFromBlack(); // Finaliza la transición desde la pantalla negra

        if(deactivateAfterStarting) // Si se debe desactivar el objeto después de iniciar la batalla
        {
            gameObject.SetActive(false); // Desactiva este objeto
        }

        // Marca la misión como completada si es necesario
        BattleReward.instance.markQuestComplete = shouldCompleteQuest;
        BattleReward.instance.questToMark = QuestToComplete;
    }
}
