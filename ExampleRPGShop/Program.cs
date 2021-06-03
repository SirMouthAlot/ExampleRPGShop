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
        //Global Variables//
        public static List<ShopItem> shopItemsList = new List<ShopItem>();
        public static List<ShopItem> playerInventory = new List<ShopItem>();

        static int playerGold = 500;
        static float sellingPriceMultiplier = 0.8f;

        static string input;
        ////////////////////
        
        static void Main(string[] args)
        {
            InitShopItems();

            StartShop();
        }

        static void InitShopItems()
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

            Console.WriteLine("1. Buy \n\n2. Sell \n\n3. Trade \n\n4. Exit \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice == 1)
                BuySection();
            else if (choice == 2)
                SellSection();
            else if (choice == 3)
                TradeSection();
            else if (choice == 4)
            {

            }
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                StartShop();
            }

        }

        static void SellSection()
        {
            if (playerInventory.Count == 0)
            {
                Console.WriteLine("You do not have any items to sell. \n");
                StartShop();
            }

            Console.WriteLine("What would you like to Sell? \n");

            for (int i = 0; i < playerInventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {playerInventory[i].GetName()} \nSelling Price: {(int)(playerInventory[i].GetValue() * sellingPriceMultiplier)} Gold \n");
            }

            Console.WriteLine($"{playerInventory.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice <= playerInventory.Count)
            {
                Console.WriteLine($"Would you like to sell {playerInventory[choice - 1].GetName()} for {(int)(playerInventory[choice - 1].GetValue() * sellingPriceMultiplier)} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                Console.Clear();

                if (confirm == 1)
                {
                    Console.WriteLine("Thank you! here is your payment\n");
                    Console.WriteLine($"Player received {(int)(playerInventory[choice - 1].GetValue() * sellingPriceMultiplier)} Gold for their {playerInventory[choice - 1].GetName()} \n");

                    for (int i = 0; i < playerInventory.Count; i++)
                    {
                        if (playerInventory[i] == playerInventory[choice - 1])
                        {
                            shopItemsList.Add(playerInventory[choice - 1]);
                            playerGold += (int)(playerInventory[choice - 1].GetValue() * sellingPriceMultiplier);
                            playerInventory.RemoveAt(i);
                        }
                    }
                }
                else if (confirm == 2)
                {
                    SellSection();
                }
                else
                {
                    Console.WriteLine("That is not a valid option.\n");
                    SellSection();
                }
            }
            else if (choice == playerInventory.Count + 1)
                StartShop();
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                SellSection();
            }

            StartShop();
        }
        static void TradeSection()
        {
            Console.WriteLine("These are your trading options");
            StartShop();
        }

        #region BUY FUNCTIONS
        static void BuySection()
        {
            Console.WriteLine($"You have {playerGold} gold \nWhat would you like to buy? \n");
            Console.WriteLine("1. Weapons \n\n2. Armour \n\n3. Consumables \n\n4. Cancel");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice == 1)
                BuyWeapon();
            else if (choice == 2)
                BuyArmour();
            else if (choice == 3)
                BuyConsumables();
            else if (choice == 4)
                StartShop();
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                BuySection();
            }

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

            Console.WriteLine($"Gold: {playerGold} \nWhich weapon would you like to buy: \n");

            for (int i = 0; i < weaponsList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {weaponsList[i].GetName()} \nPrice: {weaponsList[i].GetValue()} Gold \n");
            }

            Console.WriteLine($"{weaponsList.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice <= weaponsList.Count)
            {
                if (playerGold < weaponsList[choice - 1].GetValue())
                {
                    Console.WriteLine("You do not have enough gold, pick a different option\n");
                    BuyWeapon();
                }

                Console.WriteLine($"Would you like to buy {weaponsList[choice - 1].GetName()} for {weaponsList[choice - 1].GetValue()} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                Console.Clear();

                if (confirm == 1)
                {
                    Console.WriteLine("You have recieved your new weapon\n");

                    for (int i = 0; i < shopItemsList.Count; i++)
                    {
                        if (shopItemsList[i] == weaponsList[choice - 1])
                        {
                            playerInventory.Add(weaponsList[choice - 1]);
                            playerGold -= weaponsList[choice - 1].GetValue();
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

            Console.WriteLine($"Gold: {playerGold} \nWhich item would you like to buy: \n");

            for (int i = 0; i < armourList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {armourList[i].GetName()} \nPrice: {armourList[i].GetValue()} Gold \n");
            }

            Console.WriteLine($"{armourList.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice <= armourList.Count)
            {
                Console.WriteLine($"Would you like to buy {armourList[choice - 1].GetName()} for {armourList[choice - 1].GetValue()} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                Console.Clear();

                if (confirm == 1)
                {
                    Console.WriteLine("You have received your armour\n");

                    for (int i = 0; i < shopItemsList.Count; i++)
                    {
                        if (shopItemsList[i] == armourList[choice - 1])
                        {
                            playerInventory.Add(armourList[choice - 1]);
                            playerGold -= armourList[choice - 1].GetValue();
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

            Console.WriteLine($"Gold: {playerGold} \nWhich item would you like to buy: \n");

            for (int i = 0; i < consumablesList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {consumablesList[i].GetName()} \nPrice: {consumablesList[i].GetValue()} Gold \n");
            }

            Console.WriteLine($"{consumablesList.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice <= consumablesList.Count)
            {
                Console.WriteLine($"Would you like to buy {consumablesList[choice - 1].GetName()} for {consumablesList[choice - 1].GetValue()} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                Console.Clear();

                if (confirm == 1)
                {
                    Console.WriteLine("You have received your new item\n");

                    for (int i = 0; i < shopItemsList.Count; i++)
                    {
                        if (shopItemsList[i] == consumablesList[choice - 1])
                        {
                            playerInventory.Add(consumablesList[choice - 1]);
                            playerGold -= consumablesList[choice - 1].GetValue();
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
        #endregion
    }
}
