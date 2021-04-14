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
        state = CombatState.START;
        StartCoroutine(SetupCombat());
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

    IEnumerator SetupCombat()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerStartPos);
        playerCharacter = playerGO.GetComponent<Player>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyStartPos);
        enemyCharacter = enemyGO.GetComponent<Monster>();

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

    void PlayerTurn()
    {
        dialogueText.text = "\nChoose an Action: " + 
                            "\n1) Basic Attack" +
                            "\n2) Heal"
                            + "\n" + enemyCharacter.getName() + " - " + enemyCharacter.currentHitPoints + "/" + enemyCharacter.maxHitPoints
                            + "\t" + playerCharacter.getName() + " - " + playerCharacter.currentHitPoints + "/" + playerCharacter.maxHitPoints;
        
    }

    void EnemyTurn()
    {
        dialogueText.text += "\n It's " + enemyCharacter.getName() + "'s turn!";
        MonsterHeal(enemyCharacter);
        StartCoroutine(BasicAttack(enemyCharacter, playerCharacter));
    }

    IEnumerator BasicAttack(WorldCharacter source, WorldCharacter target)
    {
        bool canAttack = Random.Range(0.0f, 1.0f) <= source.getChanceToHit();
        int damage = 0;

        if (canAttack)
        {
            damage = (int)(Random.Range(0.0f, 1.0f) * (source.getMaxDamage() - source.getMinDamage() + 1))
                    + source.getMinDamage();
            dialogueText.text = "\n" + source.getName() + " attacks " + target.getName() + "!";

            subtractHitPoints(target, damage);
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

    void MonsterHeal(Monster source)
    {
        bool canHeal = (Random.Range(0.0f, 1.0f) <= source.getChanceToHeal()) && (source.currentHitPoints > 0);
        int healPoints = 0;

        if (canHeal)
        {
            healPoints = (int)Random.Range(source.getMinHeal(), source.getMaxHeal()+ 1.0f);
            addHitPoints(source, healPoints);
            dialogueText.text += "\n" + source.getName() + " healed itself for " + healPoints + "points.";
        }
    }

    void PlayerHeal(WorldCharacter source, WorldCharacter target)
    {
        int healPoints = 0;

        healPoints = (int)Random.Range(25, 50);
        addHitPoints(source,healPoints);
        dialogueText.text = "\n" + source.getName() + " healed for " + healPoints +
                            "\n" + target.getName() + " didn't like that!";

        state = CombatState.ENEMYTURN;
        EnemyTurn();
    }


    public void subtractHitPoints(WorldCharacter character, int hitPoints)
    {
        string result = "";

        if (hitPoints < 0)
            result = "\nHitpoint amount must be positive.";
        else if (hitPoints > 0)
        {
            character.currentHitPoints -= hitPoints;
            if (character.currentHitPoints < 0)
                character.currentHitPoints = 0;
            result += ("\n" + character.getName() + " takes <" + hitPoints + "> points of damage.");
            result += ("\n" + character.getName() + " now has " + character.getHitPoints() + " hit points remaining.");
        }

        if (character.currentHitPoints == 0)
            result += "\n" + character.getName() + " has been killed!";


        dialogueText.text += result;
    }

    public void addHitPoints(WorldCharacter source, int hitPoints)
    {
        string res = "";

        if (hitPoints <= 0)
            res = ("\nHitpoint amount must be positive.");
        else if (hitPoints + source.currentHitPoints >= source.maxHitPoints)
        {
            source.currentHitPoints = source.maxHitPoints;
            res = ("\nYou are back to full HP at: " + source.currentHitPoints);
        }
        else
        {
            source.currentHitPoints += hitPoints;
            res = ("\nRemaining Hit Points: " + source.currentHitPoints);

        }

        dialogueText.text += res;
    }

    void EndCombat()
    {
        if (state == CombatState.VICTORY)
            dialogueText.text = playerCharacter.getName() + " was victorious!";
        else if (state == CombatState.DEFEAT)
            dialogueText.text = playerCharacter.getName() + " was defeated :-(";
        state = CombatState.START;
    }
}
