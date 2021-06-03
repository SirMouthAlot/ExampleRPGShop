using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExampleRPGShop
{
    class Program
    {
        public static List<ShopItem> shopItemsList = new List<ShopItem>();
        static string input;
        //static List<ShopItem> shopItemsList;

        static void Main(string[] args)
        {

            InitShop();

            StartShop();
        }

        static void InitShop()
        {
            StreamReader reader = new StreamReader("../../ShopItems.txt");
            Random random = new Random();

            //Reads the first line from text
            string data = reader.ReadLine();

            //Continues reading lines until it reaches the end of the file
            while(data != null)
            {
                //Splits a string into an array of strings using ',' as the delimiter
                string[] shopItemsSplitAr = data.Split(',');

                //for every string in the aplit array, it splits that string into a name and a type
                for (int i = 0; i < shopItemsSplitAr.Length; i++)
                {
                    string[] itemAr = shopItemsSplitAr[i].Split(':');
                    int type = int.Parse(itemAr[1]);
                    string name = itemAr[0];

                    shopItemsList.Add(new ShopItem(name, random.Next(25, 500), type));
                }

                data = reader.ReadLine();
            }
        }

        static void StartShop()
        {
            Console.WriteLine("Welcome to our shop! What would you like to do? \n");

            Console.WriteLine("1. Buy \n\n2. Sell \n\n3. Trade \n4. Exit \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            if (choice == 1)
                BuySection();
            else if (choice == 2)
                SellSection();
            else if (choice == 3)
                TradeSection();
            else if (choice == 4)
                return;
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                StartShop();
            }

        }

        static void BuySection()
        {
            Console.WriteLine("What would you like to buy? \n");
            Console.WriteLine("1. Weapons \n\n2. Armour \n\n3. Consumables \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            if (choice == 1)
                BuyWeapon();
            else if (choice == 2)
                BuyArmour();
            else if (choice == 3)
                BuyConsumables();
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                BuySection();
            }

            StartShop();
        }
        static void SellSection()
        {
            Console.WriteLine("What would you like to Sell? \n");
            StartShop();
        }
        static void TradeSection()
        {
            Console.WriteLine("These are your trading options");
            StartShop();
        }

        static void BuyWeapon()
        {
            List<ShopItem> weaponsList = new List<ShopItem>();

            for(int i = 0; i < shopItemsList.Count; i++)
            {
                if (shopItemsList[i].GetType() == ShopItem.ItemType.Weapon)
                    weaponsList.Add(shopItemsList[i]);
            }

            Console.WriteLine("Which weapon would you like to buy: \n");

            for (int i = 0; i < weaponsList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {weaponsList[i].GetName()} \nPrice: {weaponsList[i].GetValue()} Gold \n");
            }

            Console.WriteLine($"{weaponsList.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            if (choice <= weaponsList.Count)
            {
                Console.WriteLine($"Would you like to buy {weaponsList[choice - 1].GetName()} for {weaponsList[choice - 1].GetValue()} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                if (confirm == 1)
                {
                    Console.WriteLine("Here is your new weapon. You will not get it in your inventory yet because i havent coded that part yet\n");

                    for (int i = 0; i < shopItemsList.Count; i++)
                    {
                        if (shopItemsList[i] == weaponsList[choice - 1])
                        {
                            shopItemsList.RemoveAt(i);
                        }
                    }
                }
                else if (confirm == 2)
                {
                    BuyWeapon();
                }
                else
                {
                    Console.WriteLine("That is not a valid option.\n");
                    BuyWeapon();
                }
            }
            else if (choice == weaponsList.Count + 1)
                BuySection();
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                BuyWeapon();
            }

        }

        static void BuyArmour()
        {
            List<ShopItem> armourList = new List<ShopItem>();

            for (int i = 0; i < shopItemsList.Count; i++)
            {
                if (shopItemsList[i].GetType() == ShopItem.ItemType.Armour)
                    armourList.Add(shopItemsList[i]);
            }

            Console.WriteLine("Which item would you like to buy: \n");

            for (int i = 0; i < armourList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {armourList[i].GetName()} \nPrice: {armourList[i].GetValue()} Gold \n");
            }

            Console.WriteLine($"{armourList.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            if (choice <= armourList.Count)
            {
                Console.WriteLine($"Would you like to buy {armourList[choice - 1].GetName()} for {armourList[choice - 1].GetValue()} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                if (confirm == 1)
                {
                    Console.WriteLine("Here is your armour. You will not get it in your inventory yet because i havent coded that part yet\n");

                    for (int i = 0; i < shopItemsList.Count; i++)
                    {
                        if (shopItemsList[i] == armourList[choice - 1])
                        {
                            shopItemsList.RemoveAt(i);
                        }
                    }
                }
                else if (confirm == 2)
                {
                    BuyArmour();
                }
                else
                {
                    Console.WriteLine("That is not a valid option.\n");
                    BuyArmour();
                }
            }
            else if (choice == armourList.Count + 1)
                BuySection();
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                BuyArmour();
            }
        }

        static void BuyConsumables()
        {
            List<ShopItem> consumablesList = new List<ShopItem>();

            for (int i = 0; i < shopItemsList.Count; i++)
            {
                if (shopItemsList[i].GetType() == ShopItem.ItemType.Consumable)
                    consumablesList.Add(shopItemsList[i]);
            }

            Console.WriteLine("Which item would you like to buy: \n");

            for (int i = 0; i < consumablesList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {consumablesList[i].GetName()} \nPrice: {consumablesList[i].GetValue()} Gold \n");
            }

            Console.WriteLine($"{consumablesList.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            if (choice <= consumablesList.Count)
            {
                Console.WriteLine($"Would you like to buy {consumablesList[choice - 1].GetName()} for {consumablesList[choice - 1].GetValue()} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                if (confirm == 1)
                {
                    Console.WriteLine("Here is your item. You will not get it in your inventory because i havent coded that part yet\n");

                    for (int i = 0; i < shopItemsList.Count; i++)
                    {
                        if (shopItemsList[i] == consumablesList[choice - 1])
                        {
                            shopItemsList.RemoveAt(i);
                        }
                    }
                }
                else if (confirm == 2)
                {
                    BuyConsumables();
                }
                else
                {
                    Console.WriteLine("That is not a valid option.\n");
                    BuyConsumables();
                }
            }
            else if (choice == consumablesList.Count + 1)
                BuySection();
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                BuyConsumables();
            }
        }
    }
}
