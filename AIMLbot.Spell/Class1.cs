using System;
using AIMLbot.Spell;

class Program
{
    static void Main(string[] args)
    {
        Spelling spelling = new Spelling();
        string word = "";

        word = "speling";
        Console.WriteLine("{0} => {1}", word, spelling.Correct(word));

        word = "korrecter"; // 'correcter' is not in the dictionary file so this doesn't work
        Console.WriteLine("{0} => {1}", word, spelling.Correct(word));

        word = "korrect";
        Console.WriteLine("{0} => {1}", word, spelling.Correct(word));

        word = "acess";
        Console.WriteLine("{0} => {1}", word, spelling.Correct(word));

        word = "supposidly";
        Console.WriteLine("{0} => {1}", word, spelling.Correct(word));

        // A sentence
        string sentence = "I havve speled thes woord wwrong"; // sees speed instead of spelled (see notes on norvig.com)
        string correction = "";
        foreach (string item in sentence.Split(' '))
        {
            correction += " " + spelling.Correct(item);
        }
        Console.WriteLine("Did you mean:" + correction);

        Console.Read();
    }
}