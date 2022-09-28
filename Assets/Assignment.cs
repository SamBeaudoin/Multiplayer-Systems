
/*
This RPG data streaming assignment was created by Fernando Restituto.
Pixel RPG characters created by Sean Browning.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


#region Assignment Instructions

/*  Hello!  Welcome to your first lab :)

Wax on, wax off.

    The development of saving and loading systems shares much in common with that of networked gameplay development.  
    Both involve developing around data which is packaged and passed into (or gotten from) a stream.  
    Thus, prior to attacking the problems of development for networked games, you will strengthen your abilities to develop solutions using the easier to work with HD saving/loading frameworks.

    Try to understand not just the framework tools, but also, 
    seek to familiarize yourself with how we are able to break data down, pass it into a stream and then rebuild it from another stream.


Lab Part 1

    Begin by exploring the UI elements that you are presented with upon hitting play.
    You can roll a new party, view party stats and hit a save and load button, both of which do nothing.
    You are challenged to create the functions that will save and load the party data which is being displayed on screen for you.

    Below, a SavePartyButtonPressed and a LoadPartyButtonPressed function are provided for you.
    Both are being called by the internal systems when the respective button is hit.
    You must code the save/load functionality.
    Access to Party Character data is provided via demo usage in the save and load functions.

    The PartyCharacter class members are defined as follows.  */

public partial class PartyCharacter
{
    public int classID;

    public int health;
    public int mana;

    public int strength;
    public int agility;
    public int wisdom;

    public LinkedList<int> equipment;

}


/*
    Access to the on screen party data can be achieved via …..

    Once you have loaded party data from the HD, you can have it loaded on screen via …...

    These are the stream reader/writer that I want you to use.
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader

    Alright, that’s all you need to get started on the first part of this assignment, here are your functions, good luck and journey well!
*/


#endregion


#region Assignment Part 1

static public class AssignmentPart1
{

    static public void SavePartyButtonPressed()
    {
        StreamWriter writer = new StreamWriter("PlayerParty.txt");

        Debug.Log("Party Saved!");

        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            // Save Character stats and ID on first line
            writer.Write(pc.classID + " " + pc.health + " " + pc.mana + " " + pc.strength + " " + pc.agility + " " + pc.wisdom +"\n");

            // Save Character Equipment on next line
            int count = pc.equipment.Count;

            for (int i = 0; i < count; i++)
            {
                writer.Write(pc.equipment.First.Value + " ");
                pc.equipment.RemoveFirst();
            }
            writer.Write("\n");
        }

        writer.Close();
    }

    static public void LoadPartyButtonPressed()
    {
        Debug.Log("Load Pressed!");
        
        GameContent.partyCharacters.Clear();

        StreamReader reader = new StreamReader("PlayerParty.txt");

        // Variables for split String Data
        string line;
        int ID;
        int health;
        int mana;
        int strength;
        int agility;
        int wisdom;

        PartyCharacter NewCharacter;

        while ((line = reader.ReadLine()) != null)
        {
            // Split data being stored
            string[] data = line.Split(" ");

            // Convert Data to Integers
            ID = Convert.ToInt32(data[0]);
            health = Convert.ToInt32(data[1]);
            mana = Convert.ToInt32(data[2]);
            strength = Convert.ToInt32(data[3]);
            agility = Convert.ToInt32(data[4]);
            wisdom = Convert.ToInt32(data[5]);

            // Create New Character
            NewCharacter = new PartyCharacter(ID, health, mana, strength, agility, wisdom);

            // Read/Split Equpiment Data
            line = reader.ReadLine();
            data = line.Split(" ");

            // Loop Through Saved Equipment, Ignoring Empty Value at End Of File
            for (int i = 0; i < data.Length - 1; i++)
            {
                // Add Equipment to New Character
                NewCharacter.equipment.AddFirst(Convert.ToInt32(data[i]));
            }

            // Add Loaded Character to Pool
            GameContent.partyCharacters.AddLast(NewCharacter);
        }

        GameContent.RefreshUI();

    }

}


#endregion


#region Assignment Part 2

//  Before Proceeding!
//  To inform the internal systems that you are proceeding onto the second part of this assignment,
//  change the below value of AssignmentConfiguration.PartOfAssignmentInDevelopment from 1 to 2.
//  This will enable the needed UI/function calls for your to proceed with your assignment.
static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 2;
}

/*

In this part of the assignment you are challenged to expand on the functionality that you have already created.  
    You are being challenged to save, load and manage multiple parties.
    You are being challenged to identify each party via a string name (a member of the Party class).

To aid you in this challenge, the UI has been altered.  

    The load button has been replaced with a drop down list.  
    When this load party drop down list is changed, LoadPartyDropDownChanged(string selectedName) will be called.  
    When this drop down is created, it will be populated with the return value of GetListOfPartyNames().

    GameStart() is called when the program starts.

    For quality of life, a new SavePartyButtonPressed() has been provided to you below.

    An new/delete button has been added, you will also find below NewPartyButtonPressed() and DeletePartyButtonPressed()

Again, you are being challenged to develop the ability to save and load multiple parties.
    This challenge is different from the previous.
    In the above challenge, what you had to develop was much more directly named.
    With this challenge however, there is a much more predicate process required.
    Let me ask you,
        What do you need to program to produce the saving, loading and management of multiple parties?
        What are the variables that you will need to declare?
        What are the things that you will need to do?  
    So much of development is just breaking problems down into smaller parts.
    Take the time to name each part of what you will create and then, do it.

Good luck, journey well.

*/

static public class AssignmentPart2
{

    static public void GameStart()
    {

        GameContent.RefreshUI();

    }

    static public List<string> GetListOfPartyNames()
    {
        StreamReader reader;

        // Test if file exists
        try
        {
            reader = new StreamReader("PartyNames.txt");
        }
        catch (Exception e)
        {
            return new List<string>() { "List Is Empty" };
        }

        List<string> data = new List<string>();
        string line;

        // If File Exists
        // Proccess saved file into list for displaying
        while ((line = reader.ReadLine()) != null)
        {
            data.Add(line);
        }

        reader.Close();


        return data;

    }

    static public void LoadPartyDropDownChanged(string selectedName)
    {
        Debug.Log("Loading Party: " + selectedName);

        GameContent.currentlyLoadedParty = selectedName;

        StreamReader reader = new StreamReader("PartyNames.txt");

        // Read List of Party Names
        LinkedList<string> data = new LinkedList<string>();
        string line;

        while ((line = reader.ReadLine()) != null)
        {
            data.AddLast(line);
        }

        // Check which party index is to be loaded
        int indexCount = 1;

        for (; data.First.Value != selectedName; data.RemoveFirst())
        {
            indexCount++;
        }
        reader.Close();

        GameContent.partyCharacters.Clear();

        reader = new StreamReader("Party" + (indexCount) + ".txt");

        // Variables for split String Data
        int ID;
        int health;
        int mana;
        int strength;
        int agility;
        int wisdom;

        PartyCharacter NewCharacter;

        while ((line = reader.ReadLine()) != null)
        {
            // Split data being stored
            string[] characterData = line.Split(" ");

            // Convert Data to Integers
            ID = Convert.ToInt32(characterData[0]);
            health = Convert.ToInt32(characterData[1]);
            mana = Convert.ToInt32(characterData[2]);
            strength = Convert.ToInt32(characterData[3]);
            agility = Convert.ToInt32(characterData[4]);
            wisdom = Convert.ToInt32(characterData[5]);

            // Create New Character
            NewCharacter = new PartyCharacter(ID, health, mana, strength, agility, wisdom);

            // Read/Split Equpiment Data
            line = reader.ReadLine();
            characterData = line.Split(" ");

            // Loop Through Saved Equipment, Ignoring Empty Value at End Of File
            for (int i = 0; i < characterData.Length - 1; i++)
            {
                // Add Equipment to New Character
                NewCharacter.equipment.AddFirst(Convert.ToInt32(characterData[i]));
            }

            // Add Loaded Character to Pool
            GameContent.partyCharacters.AddLast(NewCharacter);
        }
        reader.Close();

        GameContent.RefreshUI();
    }

    static public void SavePartyButtonPressed()
    {
        InputField field = GameObject.FindObjectOfType<InputField>();

        string partyName = field.text;
        GameContent.currentlyLoadedParty = partyName;

        Debug.Log("Party Name: " + partyName);

        StreamReader tempReader;
        StreamWriter tempWriter;

        // Test if file exists
        try
        {
            tempReader = new StreamReader("PartyNames.txt");
            tempReader.Close();
            tempReader = null;
        }
        catch (Exception e)
        {
            // if file does not exist, create file
            Debug.Log("File Not Found!");
            Debug.Log("Creating File!");
            tempWriter = new StreamWriter("PartyNames.txt");
            tempWriter.Close();
            tempWriter = null;
        }

        // Read previously saved data if any
        StreamReader reader = new StreamReader("PartyNames.txt");

        string line;
        LinkedList<string> data = new LinkedList<string>();


        // Iterate through file saving previous team names
        while ((line = reader.ReadLine()) != null)
        {
            data.AddLast(line);

        }
        reader.Close();

        // Add newest party to data
        data.AddLast(partyName);

        StreamWriter writer = new StreamWriter("PartyNames.txt");
        int dataCount = data.Count;

        // Proccess saved data list into file
        for (int i = 0; i < dataCount; i++)
        {
            writer.WriteLine(data.First.Value);
            data.RemoveFirst();
        }

        writer.Close();

        // Create New File for party characters
        writer = new StreamWriter("Party" + dataCount + ".txt");

        // Save party members
        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            // Save Character stats and ID on first line
            writer.Write(pc.classID + " " + pc.health + " " + pc.mana + " " + pc.strength + " " + pc.agility + " " + pc.wisdom + "\n");

            // Save Character Equipment on next line
            int count = pc.equipment.Count;

            for (int j = 0; j < count; j++)
            {
                writer.Write(pc.equipment.First.Value + " ");
                pc.equipment.RemoveFirst();
            }
            writer.Write("\n");
        }

        writer.Close();

        GameContent.RefreshUI();
    }

    static public void NewPartyButtonPressed()
    {
        // Not sure when this is called?
        Debug.Log("New Party Button Pressed!");
    }

    static public void DeletePartyButtonPressed()
    {
        // Check to see if current party is a Saved Party
        if (GameContent.currentlyLoadedParty == "")
        {
            return;
        }

        // Read List of Party Names
        StreamReader reader = new StreamReader("PartyNames.txt");
        LinkedList<string> data = new LinkedList<string>();
        string line;

        int NumberOfParties = 0;
        int indexCount = 0;
        bool partyIndexFound = false;

        while ((line = reader.ReadLine()) != null)
        {
            // Aquire All Party Names
            data.AddFirst(line);
            NumberOfParties++;

            // Find Index of Party to be Deleted
            if (partyIndexFound)
                continue;

            indexCount++;

            if (line == GameContent.currentlyLoadedParty)
            {
                // Remove party name from list
                partyIndexFound = true;
                data.RemoveFirst();
            }
        }
        reader.Close();

        // Re-Write Party Names file replacing the deleted party name with the last saved party
        StreamWriter writer = new StreamWriter("PartyNames.txt");

        int count = 0;

        while (data.Last != null)
        {
            count++;
            if(count == indexCount)
            {
                writer.WriteLine(data.First.Value);
                data.RemoveFirst();
                continue;
            }

            writer.WriteLine(data.Last.Value);
            data.RemoveLast();
        }
        writer.Close();

        // When Deleting only One party
        if (NumberOfParties == 1)
        {
            File.Delete("Party1.txt");
            Debug.Log("Single Party Deleted");
            return;
        }

        // When Deleting very last party
        if(indexCount == NumberOfParties)
        {
            File.Delete("Party" + NumberOfParties + ".txt");
            return;
        }


        // delete party's character stats
        File.Delete("Party" + indexCount + ".txt");

        // Move last party saved to deleted party's position
        File.Move("Party" + NumberOfParties + ".txt", "Party" + indexCount + ".txt");
    }
}

#endregion


