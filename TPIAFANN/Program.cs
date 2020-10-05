using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using FANNCSharp;
using FANNCSharp.Float;

namespace TPIAFANN
{


    public class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("Building example started");


            // 0 = french / 1 = spanish / 2 = english / 3 = deutsh
            string text1 = @"A Léon Werth. Je demande pardon aux enfants d'avoir dédié ce livre à une grande personne. J'ai une excuse sérieuse : cette grande personne est le meilleur ami que j'ai au monde. J'ai une autre excuse : cette grande personne peut tout comprendre, même les livres pour enfants. J'ai une troisième excuse : cette grande personne habite la France où elle a faim et froid. Elle a besoin d'être consolée. Si toutes ces excuses ne suffisent pas, je veux bien dédier ce livre à l'enfant qu'a été autrefois cette grande personne. Toutes les grandes personnes ont d'abord été des enfants. (Mais peu d'entre elles s'en souviennent.) Je corrige donc ma dédicace : A Léon Werth quand il était petit garçon";
            string text1_2 = @"A LEON WERTH Pido perdón a los niños por haber dedicado este libro a una persona mayor. Tengo una seria excusa: esta persona mayor es el mejor amigo que tengo en el mundo. Pero tengo otra excusa: esta persona mayor es capaz de comprenderlo todo, incluso los libros para niños. Tengo una tercera excusa todavía: esta persona mayor vive en Francia, donde pasa hambre y frío. Tiene, por consiguiente, una gran necesidad de ser consolada. Si no fueran suficientes todas esas razones, quiero entonces dedicar este libro al niño que fue hace tiempo esta persona mayor. Todas las personas mayores antes han sido niños. (Pero pocas de ellas lo recuerdan). Corrijo, por consiguiente, mi dedicatoria: A LEON WERTH, cuando era niño.";
            string text1_3 = @"To Leon Werth I ask the indulgence of the children who may read this book for dedicating it to a grown−up. I have a serious reason: he is the best friend I have in the world. I have another reason: this grown−up understands everything, even books about children. I have a third reason: he lives in France where he is hungry and cold. He needs cheering up. If all these reasons are not enough, I will dedicate the book to the child from whom this grown−up grew. All grown−ups were once children−− although few of them remember it. And so I correct my dedication: To Leon Werth when he was a little boy";
            string text1_4 = @"FÜR LÉON WERTH Ich bitte die Kinder um Verzeihung, daß ich dieses Buch einem Erwachsenen widme. Ich habe eine ernstliche Entschuldigung dafür: Dieser Erwachsene ist der beste Freund, den ich in der Welt habe. Ich habe noch eine Entschuldigung: Dieser Erwachsene kann alles verstehen, sogar die Bücher für Kinder. Ich habe eine dritte Entschuldigung: Dieser Erwachsene wohnt in Frankreich, wo er hungert und friert. Er braucht sehr notwendig einen Trost. Wenn alle diese Entschuldigungen nicht ausreichen, so will ich dieses Buch dem Kinde widmen, das dieser Erwachsene einst war. Alle großen Leute sind einmal Kinder gewesen (aber wenige erinnern sich daran). Ich verbessere also meine Widmung: FÜR LÉON WERTH als er noch ein Junge war";

            Dictionary<langue, string> baseText1 = new Dictionary<langue, string>();

            baseText1.Add(langue.francais, text1);
            baseText1.Add(langue.espagnol, text1_2);
            baseText1.Add(langue.anglais, text1_3);
            baseText1.Add(langue.allemand, text1_4);

            // 0 = french / 1 = spanish / 2 = english / 3 = deutsh
            string text2 = @"Lorsque j'avais six ans j'ai vu, une fois, une magnifique image, dans un livre sur la Forêt Vierge qui s'appelait Histoires Vécues. Ca représentait un serpent boa qui avalait un fauve. Voilà la copie du dessin.";
            string text2_2 = @"Cuando yo tenía seis años vi en un libro sobre la selva virgen que se titulaba Historias vividas, una magnífica lámina. Representaba una serpiente boa que se tragaba a una fiera. Esta es la copia del dibujo.";
            string text2_3 = @"Once when I was six years old I saw a magnificent picture in a book, called True Stories from Nature, about the primeval forest. It was a picture of a boa constrictor in the act of swallowing an animal. Here is a copy of the drawing.";
            string text2_4 = @"Als ich sechs Jahre alt war, sah ich einmal in einem Buch über den Urwald, das »Erlebte Geschichten« hieß, ein prächtiges Bild. Es stellte eine Riesenschlange dar, wie sie ein Wildtier verschlang. Hier ist eine Kopie der Zeichnung.";

            Dictionary<langue, string> baseText2 = new Dictionary<langue, string>();

            baseText2.Add(langue.francais, text2);
            baseText2.Add(langue.espagnol, text2_2);
            baseText2.Add(langue.anglais, text2_3);
            baseText2.Add(langue.allemand, text2_4);

            List<Dictionary<langue, string>> listDic = new List<Dictionary<langue, string>>();
            listDic.Add(baseText1);
            listDic.Add(baseText2);

            BuildExample(listDic);

            Console.WriteLine("Building example finished");

            Console.WriteLine("Starting NN training");

            // Entrainement réseau
            int connectionRate = 1;
            float learningRate = 0.7f;

            uint input = 26;
            uint hidden = 4;
            uint output = 4;

            float error = 0.00001f;
            uint iterations = 10000;
            uint report = 10;


            NeuralNet ann = new NeuralNet(connectionRate, new uint[] { input, hidden, output });
            
            ann.LearningRate = learningRate;
            ann.ActivationFunctionOutput = ActivationFunction.SIGMOID_SYMMETRIC_STEPWISE;

            ann.TrainOnFile(@"D:\Example.txt", iterations, report, error);
            ann.Save(@"D:\ResultNetwork.txt");

            Console.WriteLine("NN training ended, press enter to launch use test");
            Console.ReadLine();

            // Utilisation du NN
            NeuralNet annUse = new NeuralNet(@"D:\ResultNetwork.txt");
            string textUse = @"That, however, is not my fault. The grown-ups discouraged me in my painter's career when I was six years old, and I never learned to draw anything, except boas from the outside and boas from the inside.
Now I stared at this sudden apparition with my eyes fairly starting out of my head in astonishment. Remember, I had crashed in the desert a thousand miles from any inhabited region. And yet my little man seemed neither to be straying uncertainly among the sands, nor to be fainting from fatigue or hunger or thirst or fear. Nothing about him gave any suggestion of a child lost in the middle of the desert, a thousand miles from any human habitation. When at last I was able to speak, I said to him";

            var format = new NumberFormatInfo();
            format.NegativeSign = "-";
            format.NumberDecimalSeparator = ".";
            format.NumberDecimalDigits = 4;

            List<float> textExFreq = new List<float>();
            foreach (KeyValuePair<char, float> entryExFrq in GetFrequencyFromText(textUse))
            {
                textExFreq.Add(entryExFrq.Value);
            }

            float[] result = annUse.Run(textExFreq.ToArray());
            
            Console.WriteLine($"Francais : {result[0]} - Espagnol : {result[1]} - Anglais : {result[2]} - Allemand : {result[3]}");
            Console.ReadLine();

        }

        public static Dictionary<char, float> GetFrequencyFromText(string text)
        {
            String s = text; /*File.ReadAllText(path);*/

            Dictionary<char, int> characterCount = new Dictionary<char, int>();

            for (char c = 'A'; c <= 'Z'; c++)
            {
                characterCount.Add(c, 0);
            }

            foreach (char c in s)
            {
                if(Char.ToUpper(c) >= 'A' && Char.ToUpper(c) <= 'Z')
                {
                    if (characterCount.ContainsKey(Char.ToUpper(c)))
                    {
                        characterCount[Char.ToUpper(c)]++;
                    }
                    else
                    {
                        characterCount[Char.ToUpper(c)] = 1;
                    }
                }

            }

            Dictionary<char, float> result = new Dictionary<char, float>();
            foreach (KeyValuePair<char, int> entry in characterCount)
            {
                float freq = (float)entry.Value / s.Length;
                result.Add(entry.Key, freq);
            }

            return result;
        }

        public static void BuildExample(List<Dictionary<langue, string>> listString)
        {
            using (StreamWriter sw = new StreamWriter(@"D:\Example.txt"))
            {

                sw.WriteLine(listString.Count * (int)langue.count + " " + "26" + " " + "4" );

                foreach (Dictionary<langue, string> dic in listString)
                {
                    foreach (KeyValuePair<langue,string> entry in dic)
                    {
                        var format = new NumberFormatInfo();
                        format.NegativeSign = "-";
                        format.NumberDecimalSeparator = ".";
                        format.NumberDecimalDigits = 4;

                        string freqstr = "";
                        foreach (KeyValuePair<char, float> entryFrq in GetFrequencyFromText(entry.Value))
                        {
                            freqstr += entryFrq.Value.ToString(format) + " ";
                        }

                        sw.WriteLine(freqstr);

                        switch (entry.Key)
                        {
                            case langue.francais:
                                sw.WriteLine("1 0 0 0");
                                break;
                            case langue.espagnol:
                                sw.WriteLine("0 1 0 0");
                                break;
                            case langue.anglais:
                                sw.WriteLine("0 0 1 0");
                                break;
                            case langue.allemand:
                                sw.WriteLine("0 0 0 1");
                                break;
                        }
                    }
                }
            }
        }
    }
    public enum langue
    {
        francais,
        espagnol,
        anglais,
        allemand,
        count
    }
}
