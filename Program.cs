// comments still needed in a couple of the methods: split, createRope, traverse, concat, insert
namespace RopeStructure
{

    // Generic node class for an Rope

    public class Node
    {

        // Read/write properties

        public string S { get; set; }
        public int Length { get; set; }      // Augmented information (data)
        public Node Left { get; set; }
        public Node Right { get; set; }



        // Node constructor
        public Node(string s)
        {
            S = s;

            Length = s.Length;

            Left = Right = null;
        }
        public Node()
        {
            S = null;

            Length = 0;

            Left = Right = null;
        }
    }

    //-------------------------------------------------------------------------

    // Implementation:  rope

    class Rope
    {
        private Node root;
        const int MAX_LENGTH = 10;
        public Rope()
        {
            Node tmp = createRopeStructure("", root);
            root = tmp;
        } //Create an empty rope(1 mark).

        // Rope
        // This Methdod creates a rope with a given string
        // Parameters: string str: the string passed to createRope that will be broken up and placed into leaf nodes based on length
        public Rope(string str) // Create a balanced rope from a given string S(8 marks).
        {

            Node tmp = createRopeStructure(str, root);
            root = tmp;

        }

        Node createRopeStructure(string str, Node curr) // Create a balanced rope from a given string S
        {

            if (str == null)
            {
                Console.WriteLine("Warning you can't create a Rope with a null string");
                return default;
            }

            string leftString, rightString;
            Node tmp = new Node(str);

            if (curr != null)
            {

                tmp = curr;
            }


            if ((str.Length) > MAX_LENGTH)
            {
                //clear  parent node
                tmp.S = null;

                //set left child
                tmp.Length = (int)Math.Round((decimal)(str.Length) / 2, MidpointRounding.AwayFromZero); //This ensures length is caculated properly "remember dividing int/2 stores only whole numbers so round is needed
                leftString = str.Substring(0, tmp.Length);
                tmp.Left = new Node(leftString);

                //set right child
                rightString = str.Substring(tmp.Length, str.Length - tmp.Length); //** tmp.Length+1 was getting rid off the first the proper first letter of string 2, now it is fixed
                tmp.Right = new Node(rightString);

                curr = tmp;
                curr.Length = leftString.Length + rightString.Length;

                createRopeStructure(leftString, curr.Left);
                createRopeStructure(rightString, curr.Right);
            }
            else //case that str.length is less than 10 i.e original string is hi
            {

                curr = tmp;
                tmp.Length = (str.Length);

                tmp.S = str;

            }
            return curr;
        }

        // public TraverseInOrder
        // This Methdod traverses a tree in order and
        // then adds each string S to the referenced new concatResult string
        // Time complexity:  O(n)
        // Parameters: Node parent: where the traversal starts, concatResult: passed from concatenate, this is the full string after concatenation
        public void TraverseInOrder(Node parent, ref string concatResult)
        {
            if (parent != null) //empty rope check
            {
                TraverseInOrder(parent.Left, ref concatResult);
                concatResult += parent.S;
                TraverseInOrder(parent.Right, ref concatResult);
            }
        }

        // public Concatenate
        // Concatenate simply creates an empty newRopeS
        // then uses TraverseInOrder to simply add all S to newRopeS
        // once complete newRoot is created with parameter newRopeS
        // Time complexity:  O(n)
        // Parameters: R1: one of the 2 ropes to be concatenated, R2: the rope that will concatenate onto R1
        public Rope Concatenate(Rope R1, Rope R2)  //Return the concatenation of ropes R1 and R2
        {
            if (R1.root != null && R2.root != null) //if both r1 and r2 exist 
            {
                string newRopeS = ""; //empty string to fill
                TraverseInOrder(R1.root, ref newRopeS); //
                TraverseInOrder(R2.root, ref newRopeS);
                Rope newRoot = new Rope(newRopeS);
                return newRoot;
            }
            else //if they do not exist
            {
                Console.WriteLine("Concatenation could not occur. Either R1 or R2 are null");
                return default(Rope);
            }
        }
        // public Insert
        // Calls the private insert difference is that public 
        // Insert retrieves the original node which needs
        // to be inserted into
        // Time Complexity: O(n) because Concatenate is O(n)
        // parameters: string str: string to be inserted, int i: the index at which to split and insert at
        // Rope Original: rope which needs to be split and inserted into
        public void Insert(string str, int i)
        {
            root = Insert(str, i, root);
        }

        // private Insert
        // Insert string S at index i
        // O(n) because Concatenate is O(n)
        // parameters: string str: string to be inserted, int i: the index at which to split and insert at
        private Node Insert(string str, int i, Node node)  //Insert string S at index i(6 marks).                               
        {
            Rope R1 = new Rope(str); //create a rope with passed string 
            Node temp = new Node();
            if (!(i < 0) && !(i > node.Length - 1))
            {
                if (i == 0)
                {
                    temp.Left = R1.root;
                    temp.Right = node;
                    temp.Length = R1.root.Length + node.Length;
                }
                else if (i == node.Length - 1)
                {
                    temp.Left = node;
                    temp.Right = R1.root;
                    temp.Length = R1.root.Length + node.Length;
                }
                node = temp;
            }
            else
            {
                Rope R2 = new Rope(); //Rope used for storing R1 Split
                R2.root = root;
                Rope R3 = new Rope(); //Rope used for storing R1 Split
                Tuple<Rope, Rope> toSplit = new Tuple<Rope, Rope>(R2, R3);

                if (R2.root != null && str != null)
                {
                    toSplit = Split(toSplit.Item1.root, i);
                    R2 = toSplit.Item1;
                    R3 = toSplit.Item2;
                    R2 = Concatenate(R2, R1);
                    R1 = Concatenate(R2, R3);
                }
            }

            return node;
        }

        // Delete
        // Delete the substring S[i, j]
        // Time o(logn)
        // parameters: int i: the index of the beginning of the substring to be deleted, int j: the index of the end of the substring to be deleted

        public void Delete(int i, int j)
        {
            root = Delete(i, j, root);
        }

        // Delete
        // Delete the substring S[i, j]
        // Time o(logn)
        // parameters: int i: the index of the beginning of the substring to be deleted, int j: the index of the end of the substring to be deleted
        private Node Delete(int i, int j, Node node) // Delete the substring S[i, j] 
        {
            Rope R1 = new Rope(); //rope from main that will get the substring deleted
            Rope R2 = new Rope();
            Rope R3 = new Rope();//will hold
            Node temp = new Node();
            if ((!(i < 0) && !(i > node.Length - 1)) && (!(i < 0) && !(i > node.Length - 1)))
            {

                if ((i == 0) && (j == node.Length - 1))
                {
                    temp = new Node();
                    temp.Length = 0;

                }
                else if ((i == 0) && (j == node.Left.Length - 1))
                {
                    temp = node;
                    temp.Left = new Node();
                    temp.Length -= node.Left.Length;
                }
                else if ((i == node.Left.Length) && (j == node.Length - 1))
                {
                    temp = node;
                    temp.Right = new Node();
                    temp.Length -= node.Right.Length;
                }
                node = temp;
            }
            else //USE split... if it was working right
            {
                R3.root = node;
                R2.root = node;
                Tuple<Rope, Rope> SplitI = R3.Split(i - 1);
                Tuple<Rope, Rope> SplitJ = R2.Split(j);

                R1 = R1.Concatenate(SplitI.Item1, SplitJ.Item2);

                // node = ;
            }
            return temp;

        }

        // Public CharAt
        // calls the private CharAt
        // o(logn) average case
        // parameters: int i: the index of the character that is searched for
        public char CharAt(int i)
        {
            i = i - 1; //making it so that the first letter of the string is at index 1 and not 0 like usual
            return CharAt(root, i);
        }
        // Private CharAt
        // Return the character at index i(4 marks).
        // returns L if not found
        // o(logn) average case
        // parameters: int i: the index of the character that is searched for, Node node: holds the root of the rope it was called on
        private char CharAt(Node node, int i) //Return the character at index i(4 marks).
        {
            Char errorCode = 'L'; //error code so all code paths return a value

            if (node == null) //check for empty rope
            {
                Console.WriteLine("Cannot index an empty rope");
                return errorCode;
            }
            if (i > node.Length || i < 0) //check if index is out of bounds
            {
                Console.WriteLine("Index is out of bounds");
                return errorCode;
            }

            if (node.Length / 2 <= i && (node.Right != null)) //go right
            {
                return CharAt(node.Right, i); //traverse the tree recursively 
            }

            if (node.Length / 2 > i && (node.Left != null)) //go left
            {
                return CharAt(node.Left, i); //traverse the tree recursively
            }
            return node.S[i];

        }
        // public Split
        // Time complexity:  O(log n)
        ////returns tuple, with first rope being R1 and second being R2, using the private split method
        // Parameters: int i: index at which to split rope 

        public Tuple<Rope, Rope> Split(int i)
        {
            //returns tuple, with first rope being R1 and second being R2
            return Split(root, i);
        }
        // private Split
        // This method first traverses passed rope to be split and stores its strings
        // Split then stores all chars from 0 to passed index in one string 
        // and from passed index to last index into another string
        // once complete two new ropes are created and printed using the spilt strings
        // Time complexity:  O(log n)
        ////returns tuple, with first rope being R1 and second being R2, using the private split method
        // Parameters: int i: index at which to split rope, Node being the root of the rope to split
        private Tuple<Rope, Rope> Split(Node node, int i)
        {

            Rope R2 = new Rope(); //will hold second half of split

            //r1 is full rope, its the one that is gonna be split
            Rope R1 = new Rope();
            R1.root = root;
            Node node1 = R1.root;

            bool R2RootRight = false;
            bool R1RootRight = false;
            bool goneLeftR1 = false;
            bool goneRightR1 = false;
            bool goneLeftR2 = false;
            bool goneRightR2 = false;
            bool LeafRightR2 = false;
            bool LeafRightR1 = false;

            //path holding nodes to go into R2
            LinkedList<Node> Path = new LinkedList<Node>();
            // LinkedList<Node> Path2 = new LinkedList<Node>(); //r1


            if (node == null)//if root does not exist
            {
                Console.WriteLine("You just attempted to split an empty Rope");
            }

            else if (i > node.Length - 1 || i < 0) // if index is out bounds, or we found leaf
            {
                Console.WriteLine("Invalid split index, try again!");
            }
            else
            {
                //*****************************************
                //this is where we find the actual node containing index 
                //*****************************************

                Node curr = node;//holds current node we are looking at

                int s = curr.Length - 1; //what we compare to
                while (curr.Left != null) //if current node has a left child (means it must also have right child)
                {
                    s -= curr.Right.Length; //s holds index at each node we visit
                    if (i <= s) //if we go left
                    {

                        Path.AddLast(curr.Right); //hold the nodes we split off into R2

                        //*****************************************
                        //updating R2 tree  
                        //*****************************************
                        Node tempR2 = new Node();


                        if (!R2RootRight && !goneRightR2)//if we need to add to right child and havent gone right
                        {
                            tempR2.Right = R2.root;
                            tempR2.Right.Right = curr.Right;
                            R2RootRight = true;
                            R2.root = tempR2;
                            R2.root.Right.Length += R2.root.Right.Right.Length;
                            R2.root.Length += R2.root.Right.Length;
                        }
                        else if (R2RootRight && !goneRightR2) //if we need ti add to left child and havent gone right
                        {
                            tempR2 = R2.root;

                            tempR2.Left = new Node();//.Left;
                            tempR2.Left.Left = curr.Right;
                            R2RootRight = false;

                            R2.root = tempR2;
                            R2.root.Left.Length += R2.root.Left.Left.Length;
                            R2.root.Length += R2.root.Left.Length;
                        }
                        else if (R2RootRight && goneRightR2)//if we need to add to left child, and have gone right
                        {

                            tempR2.Right = R2.root;

                            tempR2.Left = new Node();//.Left;
                            tempR2.Left.Left = curr.Right;//.Left;
                            R2RootRight = false;
                            R2.root = tempR2;
                            R2.root.Left.Length += R2.root.Left.Left.Length;
                            R2.root.Length += R2.root.Left.Length;

                        }


                        else//here
                        {

                            tempR2.Right = R2.root;
                            tempR2.Left = new Node();//.Left;
                            tempR2.Left.Left = curr.Right; //
                            R2RootRight = true;
                            R2.root = tempR2;
                            R2.root.Left.Length += R2.root.Left.Left.Length;
                            R2.root.Length += R2.root.Left.Length;
                        }


                        goneLeftR2 = true;

                        //update R2 length
                        if (R2.root.Right == null && R2.root.Left != null)
                        {
                            R2.root.Length += R2.root.Left.Length;
                        }
                        if (R2.root.Left == null && R2.root.Right != null)
                        {
                            R2.root.Length += R2.root.Right.Length;
                        }
                        else
                        {
                            R2.root.Length = R2.root.Right.Length + R2.root.Left.Length;
                        }

                        //******* REMOVE AFTER***********
                        //this is just used for testing
                        Console.WriteLine("R2");
                        R2.PrintRope();

                        //********************finished updating r2 tree*********************


                        //*****************************************
                        ////R1 being updated now----------------------------------------
                        //*****************************************


                        //trial (going left here so setting curr.right = new node
                        if (!R1RootRight && !goneRightR1)//if we need to add to right child and havent gone right
                        {
                            Node tempR1 = R1.root;
                            tempR1.Right = new Node();//.Left;
                            R1.root = tempR1;
                            R1RootRight = true;
                        }
                        else
                        {
                            Node tempR1 = R1.root.Left;
                            tempR1.Right = new Node();//.Left;
                            R1.root.Left = tempR1;
                            R1RootRight = true;
                        }

                        goneLeftR1 = true;

                        //******* REMOVE AFTER***********
                        //this is just used for testing
                        Console.WriteLine("R1");
                        R1.PrintRope();


                        //updating the fact that the child we go next to is left child
                        LeafRightR2 = false;

                        //setting node to hold the parent of the current node
                        node = curr;
                        //setting current node to whatever side we are moving to next
                        curr = curr.Left;



                    }

                    else
                    {

                        //updating s 
                        s += (curr.Length - 1);

                        Path.AddLast(curr.Left); //hold the nodes we split off into R2

                        Console.WriteLine("right " + i);

                        //updating the fact that the child we go next to is right child for both r2
                        LeafRightR2 = true;


                        //*****************************************
                        //updating R2 tree  
                        //*****************************************
                        Node tempR2 = new Node();
                        if (!R2RootRight && !goneLeftR2) //if we need to add to right child and havent gone left
                        {
                            tempR2.Right = R2.root;
                            tempR2.Right.Right = curr.Left;//.Left;
                            R2RootRight = true;
                            R2.root = tempR2;
                            R2.root.Right.Length += R2.root.Right.Right.Length;
                            R2.root.Length += R2.root.Right.Length;

                        }
                        else if (R2RootRight && !goneLeftR2) //if we need to add to left child and havent gone left
                        {
                            tempR2 = R2.root;
                            tempR2.Left = new Node();//.Left;
                            tempR2.Left.Left = curr.Left;
                            R2RootRight = false;
                            R2.root = tempR2;
                            R2.root.Left.Length += R2.root.Left.Left.Length;
                            R2.root.Length += R2.root.Left.Length;
                        }
                        else if (R2RootRight && goneLeftR2)//if we need to add to left child, and have gone left
                        {

                            tempR2.Right = R2.root;
                            tempR2.Left = new Node();//.Left;
                            tempR2.Left.Left = curr.Left;
                            R2RootRight = false;
                            R2.root = tempR2;
                            R2.root.Left.Length += R2.root.Left.Left.Length;
                            R2.root.Length += R2.root.Left.Length;

                        }//if we need to add to right child, and have gone left
                        else
                        {

                            tempR2.Right = R2.root;
                            tempR2.Left = new Node();//.Left;
                            tempR2.Left.Left = curr.Left;
                            R2RootRight = true;
                            R2.root = tempR2;
                            R2.root.Left.Length += R2.root.Left.Left.Length;
                            R2.root.Length += R2.root.Left.Length;
                        }




                        goneRightR2 = true;


                        //update R2 length
                        if (R2.root.Right == null && R2.root.Left != null)
                        {
                            R2.root.Length += R2.root.Left.Length;
                        }
                        if (R2.root.Left == null && R2.root.Right != null)
                        {
                            R2.root.Length += R2.root.Right.Length;
                        }
                        else
                        {
                            R2.root.Length = R2.root.Right.Length + R2.root.Left.Length;
                        }

                        //******* REMOVE AFTER***********
                        //this is just used for testing
                        Console.WriteLine("R2");
                        R2.PrintRope();

                        //********************finished updating r2 tree*********************


                        //*****************************************
                        ////R1 being updated now----------------------------------------
                        //*****************************************

                        //trial (going right here so setting curr.left = new node


                        if (!R1RootRight && !goneLeftR1)//if we need to add to right child and havent gone right
                        {
                            Node tempR1 = R1.root;
                            tempR1.Left = new Node();//.Left;
                            R1.root = tempR1;
                            R1RootRight = true;

                        }

                        else
                        {
                            Node tempR1 = R1.root.Left;
                            tempR1.Left.Left = new Node();//.Left;
                            R1.root.Left = tempR1;
                            R1RootRight = true;
                        }

                        goneRightR1 = true;

                        //******* REMOVE AFTER***********
                        //this is just used for testing
                        Console.WriteLine("R1");
                        R1.PrintRope();

                        //********************finished updating r1 tree*********************


                        //setting node to hold the parent of the current node
                        node = curr;
                        //setting current node to whatever side we are moving to next
                        curr = curr.Right;

                    }



                }
                //R2.root.Length = R2.root.Right.Length + R2.root.Left.Length; //set root one more time

                Node tempR3 = R1.root.Left;
                tempR3.Left.Right.Right = new Node();//.Left;
                R1.root.Left = tempR3;

                //  R1RootRight = false;
                //******* finding the actual node containing index has been complete**********************************
                //now we have curr set to the child node containing the index, and node set to the parent of child node holding index
                //and s set to the substring last index (assuming start index to finish index could be 27 - 29) of the child node 
                //in terms of the entire string i.e string "hellotherelindsay with child node holding "there" would hold
                //in terms of child index 0 - 4, in terms of entire string a substring  of 5-9, and an s of 9 
                //we use s to update what index we want of the substring using s, so if we wanted index 6 of "hellotHerelindsay" (h) in the first place
                //index below should become 1 in "tHere" (h)
                s -= node.Length - 1;


                //if child node holding index was right of its parent
                if (LeafRightR2 == true)
                {

                    s -= node.Right.Length - 1;

                }//else if it was left of its parent

                //here is where i gets updated
                i = (s - i) - 1;

                string split1 = "";
                string split2 = "";
                /////////////////////////////////
                //if we made here, we are at parent of child containing index
                //we tried before Node tempLast = Path.Last();

                Node tempLast = curr;//this node holds the child node containing index
                //Console.WriteLine("R2");
                //R2.PrintRope();
                //Console.WriteLine("R1");
                //R1.PrintRope();

                //here we check if its splits the node
                if (i != tempLast.Length - 1 && i != 0) // if leaf node must be split
                {
                    split1 = tempLast.S.Substring(0, i - 1);//i-1 cuz starting at 0 for indexes 
                    split2 = tempLast.S.Substring(i - 1, (curr.S.Length - (i)) + 1);
                    Console.WriteLine(split1);
                    Console.WriteLine(split2);

                    //create nodes to be the new children nodes of split subtrings
                    Node split1Node = new Node(split1); //this is node holding split side which will go in R2
                    Node split2Node = new Node(split2);//will go in R1
                    tempLast = new Node();
                    tempLast.Left = split1Node;
                    tempLast.Right = split2Node;
                    tempLast.Length = tempLast.Left.Length + tempLast.Right.Length;
                    Path.RemoveLast();
                    Path.AddLast(tempLast.Right);//add new right leaf node to be split off into queue of others to be split off

                    //????fix up r2 by updating child node to be split side????

                    //Node tempR2 = R2.root;
                    //Node tempR2 = new Node();

                    if (LeafRightR2 == true)
                    {
                        Node tempR2 = new Node();

                        tempR2 = R2.root;

                        tempR2 = R2.root;
                        tempR2.Left = new Node();//.Left;
                        tempR2.Left.Left = split2Node;
                        R2RootRight = false;
                        R2.root = tempR2;
                    }
                    else
                    {
                        Node tempR4 = new Node();
                        tempR4.Right = R2.root;
                        tempR4.Left = split2Node;//.Left;
                        R2.root = tempR4;
                    }
                    if (LeafRightR1 == true)
                    {
                        Node tempR5 = R1.root.Left;
                        // tempR5.Left = new Node();
                        tempR5.Left.Right.Left = split1Node;//.Left;
                        R1.root.Left = tempR5;
                    }
                    else
                    {

                        Node tempR5 = R1.root.Left;
                        tempR5.Left.Right.Left = split1Node;//.Left;
                        R1.root.Left = tempR5;
                    }



                }

                R2.root.Length = R2.root.Right.Length + R2.root.Left.Length;

                Console.WriteLine("R2");
                R2.PrintRope();

                Console.WriteLine("R1");
                R1.PrintRope();





            }
            Tuple<Rope, Rope> R1R2 = new Tuple<Rope, Rope>(R1, R2);
            return R1R2;
        }



        // public IndexOf
        // calls the private IndexOf
        // o(logn) average case, o(n) worst case
        // parameters: char c: this is the char that the method searches for
        // returns a call for private IndexOf containing the char c and the root of the rope it was called on
        public int IndexOf(char c)
        {
            return IndexOf(c, root);
        }

        // Private IndexOf
        // Return the index of the first occurrence of character c
        // index is calculated by tracking and adding all leaf nodes prior to and up to the index we are looking for
        // returns -1 if not found
        // o(logn) average case, o(n) worst case
        // parameters: char c: this is the char that the method searches for, Node node: this is the root of the rope where we start the search
        public int IndexOf(char c, Node node)
        {
            //Declare a LinkedList to keep track of previous parents while traversing
            LinkedList<Node> parents = new LinkedList<Node>();
            Node curr = node;
            int index = 0;
            //Check for empty node before looking for index
            if (node.Length == 0)
            {
                Console.WriteLine("You just attempted to index an empty Rope");
                return -1;
            }

            else
            {
                if (curr != null) //exception check for empty node
                {
                    while (parents != null) //Loops until all parents have been visited or char is found
                    {
                        //Go to bottom left parent which contains leaf children
                        while (curr.Left != null && curr.Left.S == null)
                        {
                            parents.AddLast(curr);
                            curr = curr.Left;

                        }
                        //If parent.Left contains c then return index
                        if (curr.Left.S.Contains(c))
                        {
                            index += curr.Left.S.IndexOf(c);
                            return index;
                        }
                        //If parent.Right contains c then return index
                        else if (curr.Right.S.Contains(c))
                        {
                            index += curr.Left.Length + curr.Right.S.IndexOf(c);
                            return index;
                        }
                        else
                        { //increase the index as we continue traversing
                            index += curr.Length;
                            if (parents.Last == null)
                            { //check if all nodes have been visited
                                Console.WriteLine("Character {0} was not found!", c);
                                return -1;
                            }
                            //Move back to prior node
                            curr = parents.Last();
                            //Remove last node we worked on from the LinkedList
                            parents.RemoveLast();
                            //Set cuu to curr.Right since out algorithm always moves left
                            curr = curr.Right;
                        }
                    }
                }
            }
            //we get here if char c is not found
            return -1;
        }

        // Public Reverse
        // public reverse that calls the private reverse
        // Time complexity:  O(n)
        public void reverse()
        {
            reverse(root);
        }
        // Public Reverse
        // Reverse the string represented by the current rope
        // Time complexity:  O(n)
        // Parameters: Node node: the root of the rope
        private void reverse(Node node)
        {
            Node curr = node;
            if (curr == null) //empty rope check
            {
                Console.WriteLine("You just attempted to reverse an empty Rope");
            }
            else if (curr.Left != null) //until at end of rope
            {
                Node temp = curr.Left; //temp variable to hold left
                curr.Left = curr.Right; //put right into left
                curr.Right = temp; //put temp (which is holding left) into right, swapping the nodes
                reverse(curr.Left); //go again until complete traversal has been completed
                reverse(curr.Right);
            }
        }


        // public ToString
        // This is the public method which Returns the string represented by the current rope(4 marks).
        // calls the private ToString() to do its job
        // Time complexity:  O(logn)
        //
        public string ToString()
        {
            return ToString(root);
        }

        // private ToString
        // This is the private method which Returns the string represented by the current rope(4 marks).
        // calls the private ToString() to do its job
        // Time complexity:  O(logn)
        // Parameters: node: root node of the rope used to traverse to the leafs
        private string ToString(Node node)
        {
            string str = "";//create empty string to hold rope string

            if (node != null) //empty rope check
            {

                //if node has a left child that is not a leaf node
                if (node.Left != null && node.Left.S == null)
                {

                    str += ToString(node.Left);//check next left node
                    str += ToString(node.Right);//check next right node
                }
                else //node is parent of leaf nodes
                {
                    //print leaf nodes

                    //check strings are not null
                    if (node.Left.S != null)
                    {
                        str += node.Left.S; //add left string to master string
                    }

                    //check strings are not null
                    if (node.Right.S != null)
                    {
                        str += node.Right.S; //add right string to master string
                    }

                }

            }

            return str;
        }


        // Substring()
        // This is the private method which Returns the substring from start index i to end index j of full string
        // represented by the rope(4 marks).
        // Time complexity:  O(n)
        // Parameters: i: the starting index of the substring, j: the ending index of the substring
        public string Substring(int i, int j)
        {
            string substring = ""; //empty substring to fill
            if (root == null) //empty rope check
            {
                Console.WriteLine("You just attempted to Substring an empty Rope");
            }
            //if i or j are out of bounds, of if starting index is greater than ending index
            else if ((i > root.Length || i < 0) || (j > root.Length || j < 0) || i > j)
            {
                Console.WriteLine("Invalid Substring index, try again!");
            }
            else //If neither rope provided are empty ropes and the index is inbounds
            {
                string newRopeS = "";
                TraverseInOrder(root, ref newRopeS); //call traverse in order to turn the rope into a string that can be read and indexed
                substring = newRopeS.Substring(i, j); //index the string

            }
            return substring;


        }

        //Length
        //Return the length of the string (1 mark).
        //O(1)
        public int Length()
        {
            if (root == null)
            {
                return 0;
            }
            return root.Length; //already set by the getters and setters for Node
        }


        // Public Height
        // Returns the height of the Augmented Treap
        // Calls Private Height to carry out the actual calculation
        // Time complexity:  O(n)

        public int Height()
        {
            return Height(root);
        }

        // Private Height
        // Returns the height of the given Augmented Treap
        // Time complexity:  O(n)
        // Parameters: Node root: base/root of the rope where it can count the height
        private int Height(Node root)
        {
            if (root == null)
                return -1;    // By default for an empty Augmented Treap
            else
                return 1 + Math.Max(Height(root.Left), Height(root.Right));
        }

        // Public Print
        // Prints out the items of the Augmented Treap inorder
        // Calls Private Print to carry out the actual print

        public void PrintRope()
        {
            PrintRope(root, 0);
        }

        // PrintRope
        // Inorder traversal of the Augmented Treap
        // Time complexity:  O(n)
        // parameters: Node root: root of the rope, int index: starting at 0, split the string into the different nodes by index + 8
        private void PrintRope(Node root, int index)
        {
            if (root != null) //empty rope check, continue if not
            {
                PrintRope(root.Right, index + 8); //recursively splitting each node by index + 8 going right
                if (root.Right == null) //Check for root right
                {
                    Console.WriteLine(new String(' ', index) + root.S + " " + root.Length.ToString());
                }
                else //Check for root left
                {
                    Console.WriteLine(new String(' ', index) + root.Length.ToString());
                }
                PrintRope(root.Left, index + 8); //recursively splitting each node by index + 8 going left
            }
        }

    }

    //-----------------------------------------------------------------------------

    public class Program
    {

        static void Main(string[] args)
        {

            string s = "WhyDoesThisThingKeepGoingNuclearWitTheLoop";
            string t = "ThisIsTheStartOfStringNumberTwoAndItLooksLikeConcatWorksGreatZANDDDDDDDDDITLOOKS GREAT YES IT DOES";
            Rope rope1 = new Rope(t);
            Rope rope3 = new Rope(t);
            Rope r2 = new Rope();
            Rope r3 = new Rope();

            //Split
            rope1.PrintRope();
            rope1.Split(15);

            //insert()
            rope1.Insert("Insert", 6);

            //indexOf()
            Console.WriteLine("index of i:" + rope3.IndexOf('i'));
            Console.WriteLine("index of Z:" + rope3.IndexOf('Z'));

            //charat()
            Console.WriteLine(rope3.ToString());
            Console.WriteLine("Char at index 7: " + rope3.CharAt(7));

        }
    }
}
