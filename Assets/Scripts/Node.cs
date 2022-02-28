using UnityEngine;

[System.Serializable]
public class Node : INode<Node>
{
    //부모 노드
    public Node Parent;

    public int X; // { get; set; }
    public int Y;// { get; set; }
    public Vector3 Position;// { get; set; }
    public bool Walkable; // { get; set; }

    public int Gcost { get; set; } 
    public int Hcost { get; set; } 
    public int Fcost { get => Gcost + Hcost; }

    public int HeapIndex { get; set; }

    public Node(Vector3 position, int x, int y, bool walkable)
    {
        Position = position;
        X = x;
        Y = y;
        Walkable = walkable;
    }

    public int CompareTo(Node other)
    {
        //내 Fcost 숫자가 높으면 1
        //같으면 0
        //내 Fcost 숫자가 낮으면 -1
        //숫자 낮은게 우선순위가 높음
        int compare = Fcost.CompareTo(other.Fcost);

        //Fcost가 같았을경우
        if (compare == 0)
        {
            compare = Hcost.CompareTo(other.Hcost);
        }


        return compare;
    }
}