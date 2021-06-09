using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum CombatState
    {
        START,
        PLAYERTURN,
        ENEMYTURN,
        VICTORY,
        DEFEAT
    }

public class CombatSystem : MonoBehaviour
{

    public CombatState state;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerStartPos;
    public Transform enemyStartPos;

    Player playerCharacter;
    Monster enemyCharacter;

    public Text dialogueText;

    // Starts combat between player and enemies
    void Start()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerStartPos);
        playerCharacter = playerGO.GetComponent<Player>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyStartPos);
        enemyCharacter = enemyGO.GetComponent<Monster>();

        //StartCoroutine(SetupCombat());
    }

    // Update is called once per frame
    void Update()
    {
        if(state == CombatState.PLAYERTURN)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                StartCoroutine(BasicAttack(playerCharacter, enemyCharacter));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                PlayerHeal(playerCharacter, enemyCharacter);
            }
        }
    }

    //This sets up combat by getting both player and enemy GameObjects, introducing the enemy and then giving the starting turn to
    //whichever character has the faster attack speed
    public IEnumerator SetupCombat()
    {
        state = CombatState.START;

        dialogueText.text = "Watch out! Here comes a " + enemyCharacter.name;

        yield return new WaitForSeconds(2f);

        if (playerCharacter.attackSpeed > enemyCharacter.attackSpeed)
        {
            state = CombatState.PLAYERTURN;
            PlayerTurn();
        }
        else
        {
            state = CombatState.ENEMYTURN;
            EnemyTurn();
        }

    }

    //Prints a list of options for the player to choose, can be expanded by adding an option (i.e. "x) Special attack") below and asigning the proper key press in the Update() function.
    void PlayerTurn()
    {
        dialogueText.text = "\nChoose an Action: " + 
                            "\n1) Basic Attack" +
                            "\n2) Heal"
                            + "\n" + enemyCharacter.getName() + " - " + enemyCharacter.currentHitPoints + "/" + enemyCharacter.maxHitPoints
                            + "\t" + playerCharacter.getName() + " - " + playerCharacter.currentHitPoints + "/" + playerCharacter.maxHitPoints;
        
    }

    //Handles the enemy's turn by annoucing the turn, executing the Enemy heal method for a chance for the monster to heal and 
    //then exectuing the basic attack method. This should be changed in the future to have each different enemy class havbe thier own turn method so we can customize 
    //each enemy even more
    void EnemyTurn()
    {
        dialogueText.text += "\nIt's " + enemyCharacter.getName() + "'s turn!";
        EnemyHeal(enemyCharacter);
        StartCoroutine(BasicAttack(enemyCharacter, playerCharacter));
    }


    //This basic attack function calculates a successful attack(canAttack) by getting the attackers chance to hit(same as chance to heal in the EnemyHeal() method) 
    //The damage is a random int determined by the attackers min and max damage as the range.
    // After the attack ot checks if the defender is still alive if not the combat is over, if they are it switches to the other characters turn.
    IEnumerator BasicAttack(WorldCharacter source, WorldCharacter target)
    {
        bool canAttack = Random.Range(0.0f, 1.0f) <= source.getChanceToHit();
        int damage = 0;

        if (canAttack)
        {
            damage = (int)(Random.Range(0.0f, 1.0f) * (source.getMaxDamage() - source.getMinDamage() + 1))
                    + source.getMinDamage();
            dialogueText.text = "\n" + source.getName() + " attacks " + target.getName() + "!";

             dialogueText.text  += target.subtractHitPoints(damage);
        }
        else
        {
            dialogueText.text += "\n" +source.getName() + "'s attack on " + target.getName() + " failed!";
        }

        yield return new WaitForSeconds(3f);

        if (!target.isAlive())
        {
            state = CombatState.VICTORY;
            EndCombat();
        }
        else if(state == CombatState.PLAYERTURN)
        {
            state = CombatState.ENEMYTURN;
            EnemyTurn();
        }else if(state == CombatState.ENEMYTURN)
        {
            state = CombatState.PLAYERTURN;
            PlayerTurn();
        }
    }

    //this heals the monster IF the random double is less than the chance to heal that the enemy holds. if the enemy
    //can heal, the amount healed is a random int bewteen the enemy's min and max heal(these attributes are stored in the
    //enemy class
    void EnemyHeal(Monster source)
    {
        bool canHeal = (Random.Range(0.0f, 1.0f) <= source.getChanceToHeal()) && (source.currentHitPoints > 0);
        int healPoints = 0;

        if (canHeal)
        {
            healPoints = (int)Random.Range(source.getMinHeal(), source.getMaxHeal()+ 1.0f);
            source.addHitPoints(healPoints);
            dialogueText.text += "\n" + source.getName() + " healed itself for " + healPoints + "points.";
        }
    }

    //this Heals the player a random amount between 25 and 50, can be changed for balancing needs
    void PlayerHeal(WorldCharacter source, WorldCharacter target)
    {
        int healPoints = 0;

        healPoints = (int)Random.Range(25, 50);
        source.addHitPoints(healPoints);
        dialogueText.text = "\n" + source.getName() + " healed for " + healPoints +
                            "\n" + target.getName() + " didn't like that!";

        state = CombatState.ENEMYTURN;
        EnemyTurn();
    }

    //prints out a result string if player lost or won and resets state of the battle
    void EndCombat()
    {
        if (state == CombatState.VICTORY)
            dialogueText.text = playerCharacter.getName() + " was victorious!";
        else if (state == CombatState.DEFEAT)
            dialogueText.text = playerCharacter.getName() + " was defeated :-(";
        state = CombatState.START;
    }
}
