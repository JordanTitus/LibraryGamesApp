using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryGamesApp.Utilities_Folder
{
    /// <summary>
    /// RedBlack Tree was used from vc learn and altered according to my needs and to match this application
    /// </summary>
    enum RedBlack
    {
        Red,
        Black
    }
    internal class RedBlackTree
    {
        public class Node
        {
            public RedBlack colour;
            public Node left;
            public Node right;
            public Node parent;
            public int data;
            public string desc;

            public Node(int data) { this.data = data; }
            public Node(RedBlack colour) { this.colour = colour; }
            public Node(int data, RedBlack colour) { this.data = data; this.colour = colour; }
            public Node(int data, string desc) { this.data = data; this.desc = desc; }

        }

        private Node root;

        public RedBlackTree()
        {
        }

        private void LeftRotate(Node X)
        {
            if (X != null && X.right != null) // Check if X and X's right child are not null
            {
                Node Y = X.right; // set Y
                X.right = Y.left; // turn Y's left subtree into X's right subtree

                if (Y.left != null)
                {
                    Y.left.parent = X;
                }

                if (Y != null)
                {
                    Y.parent = X.parent; // link X's parent to Y
                }

                if (X.parent == null)
                {
                    root = Y;
                }
                else if (X == X.parent.left)
                {
                    X.parent.left = Y;
                }
                else
                {
                    X.parent.right = Y;
                }

                Y.left = X; // put X on Y's left

                if (X != null)
                {
                    X.parent = Y;
                }
            }
        }

        private void RightRotate(Node Y)
        {
            // right rotate is simply mirror code from left rotate
            Node X = Y.left;
            Y.left = X.right;
            if (X.right != null)
            {
                X.right.parent = Y;
            }
            if (X != null)
            {
                X.parent = Y.parent;
            }
            if (Y.parent == null)
            {
                root = X;
            }
            if (Y == Y.parent.right)
            {
                Y.parent.right = X;
            }
            if (Y == Y.parent.left)
            {
                Y.parent.left = X;
            }

            X.right = Y;//put Y on X's right
            if (Y != null)
            {
                Y.parent = X;
            }
        }

        

        public Node Find(int key)
        {
            bool isFound = false;
            Node temp = root;
            Node item = null;

            while (!isFound && temp != null)
            {
                if (key < temp.data)
                {
                    temp = temp.left;
                }
                else if (key > temp.data)
                {
                    temp = temp.right;
                }
                else // key == temp.data
                {
                    isFound = true;
                    item = temp;
                }
            }

            if (isFound)
            {
                //Console.WriteLine("{0} was found", key);
                return item;
            }
            else
            {
                //Console.WriteLine("{0} not found", key);
                return null;
            }
        }

        public void Insert(int item, string desc)
        {
            Node newItem = new Node(item, desc);
            if (root == null)
            {
                root = newItem;
                root.colour = RedBlack.Black;
                return;
            }
            Node Y = null;
            Node X = root;
            while (X != null)
            {
                Y = X;
                if (newItem.data < Y.data)
                {
                    X = Y.left;
                }
                else
                {
                    X = Y.right;
                }
            }
            newItem.parent = Y;
            if (Y == null)
            {
                root = newItem;
            }
            else if (newItem.data < Y.data)
            {
                Y.left = newItem;
            }
            else
            {
                Y.right = newItem;
            }
            newItem.left = null;
            newItem.right = null;
            newItem.colour = RedBlack.Red; // colour the new node red
            InsertFixUp(newItem); // call method to check for violations and fix
        }

        

        

        private void InsertFixUp(Node item)
        {
            //Checks Red-Black Tree properties
            while (item != root && item.parent.colour == RedBlack.Red)
            {
                /*We have a violation*/
                if (item.parent == item.parent.parent.left)
                {
                    Node uncle = item.parent.parent.right;

                    if (uncle != null && uncle.colour == RedBlack.Red)//Case 1: uncle is red
                    {
                        item.parent.colour = RedBlack.Black;
                        uncle.colour = RedBlack.Black;
                        item.parent.parent.colour = RedBlack.Red;
                        item = item.parent.parent;
                    }
                    else //Case 2: uncle is black
                    {
                        if (item == item.parent.right)
                        {
                            item = item.parent;
                            LeftRotate(item);
                        }
                        //Case 3: recolour & rotate
                        item.parent.colour = RedBlack.Black;
                        item.parent.parent.colour = RedBlack.Red;
                        RightRotate(item.parent.parent);
                    }
                }
                else // Mirror case
                {
                    Node uncle = item.parent.parent.left;

                    if (uncle != null && uncle.colour == RedBlack.Red) //Case 1: uncle is red
                    {
                        item.parent.colour = RedBlack.Black;
                        uncle.colour = RedBlack.Black;
                        item.parent.parent.colour = RedBlack.Red;
                        item = item.parent.parent;
                    }
                    else //Case 2: uncle is black
                    {
                        if (item == item.parent.left)
                        {
                            item = item.parent;
                            RightRotate(item);
                        }
                        //Case 3: recolour & rotate
                        item.parent.colour = RedBlack.Black;
                        item.parent.parent.colour = RedBlack.Red;
                        LeftRotate(item.parent.parent);
                    }
                }
            }

            root.colour = RedBlack.Black; // Always re-color the root black after fixing violations
        }


        public void Delete(int key)
        {
            //first find the node in the tree to delete and assign to item pointer/reference
            Node item = Find(key);
            Node X = null;
            Node Y = null;

            if (item == null)
            {
                Console.WriteLine("Nothing to delete!");
                return;
            }
            if (item.left == null || item.right == null)
            {
                Y = item;
            }
            else
            {
                Y = TreeSuccessor(item);
            }
            if (Y.left != null)
            {
                X = Y.left;
            }
            else
            {
                X = Y.right;
            }
            if (X != null)
            {
                X.parent = Y;
            }
            if (Y.parent == null)
            {
                root = X;
            }
            else if (Y == Y.parent.left)
            {
                Y.parent.left = X;
            }
            else
            {
                Y.parent.left = X;
            }
            if (Y != item)
            {
                item.data = Y.data;
            }
            if (Y.colour == RedBlack.Black)
            {
                DeleteFixUp(X);
            }

        }

        private void DeleteFixUp(Node X)
        {

            while (X != null && X != root && X.colour == RedBlack.Black)
            {
                if (X == X.parent.left)
                {
                    Node W = X.parent.right;
                    if (W.colour == RedBlack.Red)
                    {
                        W.colour = RedBlack.Black; //case 1
                        X.parent.colour = RedBlack.Red; //case 1
                        LeftRotate(X.parent); //case 1
                        W = X.parent.right; //case 1
                    }
                    if (W.left.colour == RedBlack.Black && W.right.colour == RedBlack.Black)
                    {
                        W.colour = RedBlack.Red; //case 2
                        X = X.parent; //case 2
                    }
                    else if (W.right.colour == RedBlack.Black)
                    {
                        W.left.colour = RedBlack.Black; //case 3
                        W.colour = RedBlack.Red; //case 3
                        RightRotate(W); //case 3
                        W = X.parent.right; //case 3
                    }
                    W.colour = X.parent.colour; //case 4
                    X.parent.colour = RedBlack.Black; //case 4
                    W.right.colour = RedBlack.Black; //case 4
                    LeftRotate(X.parent); //case 4
                    X = root; //case 4
                }
                else //mirror code from above with "right" & "left" exchanged
                {
                    Node W = X.parent.left;
                    if (W.colour == RedBlack.Red)
                    {
                        W.colour = RedBlack.Black;
                        X.parent.colour = RedBlack.Red;
                        RightRotate(X.parent);
                        W = X.parent.left;
                    }
                    if (W.right.colour == RedBlack.Black && W.left.colour == RedBlack.Black)
                    {
                        W.colour = RedBlack.Black;
                        X = X.parent;
                    }
                    else if (W.left.colour == RedBlack.Black)
                    {
                        W.right.colour = RedBlack.Black;
                        W.colour = RedBlack.Red;
                        LeftRotate(W);
                        W = X.parent.left;
                    }
                    W.colour = X.parent.colour;
                    X.parent.colour = RedBlack.Black;
                    W.left.colour = RedBlack.Black;
                    RightRotate(X.parent);
                    X = root;
                }
            }
            if (X != null)
                X.colour = RedBlack.Black;
        }

        private Node Minimum(Node X)
        {
            while (X.left.left != null)
            {
                X = X.left;
            }
            if (X.left.right != null)
            {
                X = X.left.right;
            }
            return X;
        }

        private Node TreeSuccessor(Node X)
        {
            if (X.left != null)
            {
                return Minimum(X);
            }
            else
            {
                Node Y = X.parent;
                while (Y != null && X == Y.right)
                {
                    X = Y;
                    Y = Y.parent;
                }
                return Y;
            }
        }

        // Method to read data from a text file and populate the tree
        public void PopulateTreeFromFile(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('.');
                    if (parts.Length >= 2)
                    {
                        int key = int.Parse(parts[0]);
                        string desc = parts[1];
                        Insert(key, desc);
                    }
                }

                
                Console.WriteLine("Read success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }

        }

        public List<string>[] DisplayTree()
        {
            List<string>[] levels = new List<string>[3];
            levels[0] = new List<string>(); // First level
            levels[1] = new List<string>(); // Second level
            levels[2] = new List<string>(); // Third level

            if (root == null)
            {
                Console.WriteLine("Nothing in the tree!");
                
            }
            if (root != null)
            {
                InOrderDisplay(root);
                DetermineLevel(root, levels, 0);
            }

            return levels;
        }

        private void InOrderDisplay(Node current)
        {
            if (current != null)
            {
                InOrderDisplay(current.left);

                // Check if the current node is a left child, right child, or the root
                string position = "";
                if (current.parent != null)
                {
                    position = current.parent.left == current ? " (Left) " : " (Right) ";
                }
                else
                {
                    position = " (Root) ";
                }

                Console.WriteLine("({0}) {1} ({2}) ({3})", current.data, position, current.colour, current.desc);

                InOrderDisplay(current.right);


            }

        }

        public List<string> GetTreeValues()
        {
            List<string> values = new List<string>();
            InOrderTraversal(root, values);
            return values;
        }

        private void InOrderTraversal(Node current, List<string> values)
        {
            if (current != null)
            {
                InOrderTraversal(current.left, values);
                values.Add(current.data.ToString()); 
                InOrderTraversal(current.right, values);
            }

        }

        

        public List<string>[] GetLevels()
        {
            List<string>[] levels = new List<string>[3];
            levels[0] = new List<string>(); // First level
            levels[1] = new List<string>(); // Second level
            levels[2] = new List<string>(); // Third level

            DetermineLevel(root, levels, 0);

            return levels;
        }

        private void DetermineLevel(Node current, List<string>[] levels, int level)
        {
            if (current != null)
            {
                if (current.data % 10 == 0)
                {
                    if (current.data % 100 == 0)
                    {
                        levels[0].Add(current.data + "." + current.desc); // First level
                        level = 0;
                    }
                    else
                    {
                        levels[1].Add(current.data + "." + current.desc); // Second level
                        level = 1;
                    }
                }
                else
                {
                    levels[2].Add(current.data + "." + current.desc); // Third level
                    level = 2;
                }

                DetermineLevel(current.left, levels, level);
                DetermineLevel(current.right, levels, level);
            }

            
        }
    }
}
//-------------------------------------------EndOfFile-----------------------------------------//
