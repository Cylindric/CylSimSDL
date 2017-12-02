﻿namespace Engine.Pathfinding
{
    public class Path_Node<T> {

        public T data;

        public Path_Edge<T>[] edges;	// Nodes leading OUT from this node.
    }
}
