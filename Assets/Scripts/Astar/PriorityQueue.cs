using System;
public interface INode<T> : IComparable<T>
{
    int HeapIndex { get; set; } // 몇번째 자리에 있는지
}

public class PriorityQueue<T> where T : INode<T>
{
    private readonly T[] nodes;
    private int nodeIndex;
    public int Count { get => nodeIndex - 1; }

    public PriorityQueue(int maxHeapSize)
    {
        nodeIndex = 1;
        nodes = new T[maxHeapSize + 1];
    }

    public void Push(T node)
    {
        node.HeapIndex = nodeIndex;
        nodes[nodeIndex] = node;
        SortUp(node);
        nodeIndex++;
    }

    private void SortUp(T node)
    {
        int parentIndex = 0;

        while ((parentIndex = GetParentIndex(node.HeapIndex)) != 0)
        {
            T parentNode = nodes[parentIndex];

            if (node.CompareTo(parentNode) < 0)
            {
                Swap(node, parentNode);
            }
            else 
            {
                break;
            }
        }
    }

    public T Pop()
    {
        T firstNode = nodes[1];
        nodeIndex--;
        nodes[1] = nodes[nodeIndex];
        nodes[1].HeapIndex = 1;

        SortDown(nodes[1]);

        return firstNode;
    }

    private void SortDown(T node)
    {
        int childIndex = 0;

        //자식이 없을때 까지 while문
        while ((childIndex = GetChildIndex(node.HeapIndex)) != 0)
        {
            //자식이 둘 있을경우 우선순위가 높은 아이
            //자식이 하나 있을경우 왼쪽 자식 노드
            T childNode = nodes[childIndex];

            //내가 우선순위가 낮을 경우 내려감
            if (node.CompareTo(childNode) > 0)
            {
                //자리 바꿈
                Swap(node, childNode);
            }
            else // 내가 우선순위가 높거나 같으면
            {
                //while문 깨고 나감
                break;
            }
        }
    }

    private void Swap(T nodeA, T nodeB)
    {
        nodes[nodeA.HeapIndex] = nodeB;
        nodes[nodeB.HeapIndex] = nodeA;

        int tempIndex = nodeA.HeapIndex;
        nodeA.HeapIndex = nodeB.HeapIndex;
        nodeB.HeapIndex = tempIndex;
    }

    private int GetParentIndex(int selfIndex)
    {
        return selfIndex / 2;
    }

    private int GetChildIndex(int selfIndex)
    {
        if (GetLeftChildIndex(selfIndex) > Count)
        {
            return 0;
        }
        else if (GetLeftChildIndex(selfIndex) == Count)
        {
            return GetLeftChildIndex(selfIndex);
        }
        else
        {
            T leftChildNode = nodes[GetLeftChildIndex(selfIndex)];
            T rightChildNode = nodes[GetRightChildIndex(selfIndex)];

            if ((leftChildNode.CompareTo(rightChildNode)) < 0)
            {
                return GetLeftChildIndex(selfIndex);
            }
            else 
            {
                return GetRightChildIndex(selfIndex);
            }
        }
    }

    private int GetLeftChildIndex(int selfIndex)
    {
        return selfIndex * 2;
    }

    private int GetRightChildIndex(int selfIndex)
    {
        return selfIndex * 2 + 1;
    }

    //nodes에 있나 없나
    public bool Contains(T node)
    {
        //nodes에 들어온 node랑 비교
        return Equals(nodes[node.HeapIndex], node);
    }
}
