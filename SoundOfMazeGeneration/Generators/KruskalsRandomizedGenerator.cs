﻿using SoundOfMazeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundOfMazeGeneration.Generators
{
    public class KruskalsRandomizedGenerator : BaseGenerator
    {
        public override string Name => "Randomized Kruskal's Algorithm";

        override public int RecommendedTimeStep => 20;
        private List<HashSet<Cell>> _sets = new List<HashSet<Cell>>();
        private HashSet<Edge> _edges = new HashSet<Edge>();
        private Edge _currentEdge;
        public KruskalsRandomizedGenerator(Maze maze) : base(maze)
        {
            foreach (var cell in maze.Cells)
            {
                if (cell.Neighbours.ContainsKey(Direction.East)) _edges.Add(new Edge() { Cell = cell, Direction = Direction.East });
                if (cell.Neighbours.ContainsKey(Direction.South)) _edges.Add(new Edge() { Cell = cell, Direction = Direction.South });
            }

        }

        override public Cell NextStep()
        {

            if (_currentEdge == null)
            {
                HashSet<Cell> setA, setB;
                Edge edge;
                Cell neighbour;
                do
                {
                    if (_edges.Count == 0) return null;

                    edge = _edges.RandomElement(_rand);
                    _edges.Remove(edge);
                    neighbour = edge.Cell.Neighbours[edge.Direction];
                    setA = _sets.FirstOrDefault(set => set.Contains(edge.Cell)) ?? new HashSet<Cell>() { edge.Cell };
                    setB = _sets.FirstOrDefault(set => set.Contains(neighbour)) ?? new HashSet<Cell>() { neighbour };
                }
                while (setA == setB && setA != null);
                neighbour.CellState = edge.Cell.CellState = CellState.Visiting;
                _currentEdge = edge;
                return edge.Cell;
            }
            else
            {
                var edge = _currentEdge;
                _currentEdge = null;
                var neighbour = edge.Cell.Neighbours[edge.Direction];
                var setA = _sets.FirstOrDefault(set => set.Contains(edge.Cell)) ?? new HashSet<Cell>() { edge.Cell };
                var setB = _sets.FirstOrDefault(set => set.Contains(neighbour)) ?? new HashSet<Cell>() { neighbour };

                if (setA != setB)
                {
                    edge.Cell.Tunnel(edge.Direction);
                    _sets.Remove(setB);
                    setA.UnionWith(setB);
                    if (!_sets.Contains(setA)) _sets.Add(setA);
                }
                AddStep(neighbour);
                AddStep(edge.Cell);
                return neighbour;
            }
        }

        class Edge
        {
            public Cell Cell { get; set; }
            public Direction Direction { get; set; }
        }
    }
}
