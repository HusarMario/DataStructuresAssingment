using DataStructuresAssignment.Structures.Implementations;
using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Tester.Structures.Implementations;
using DataStructuresAssignment.Tester.Structures.Interfaces;

namespace DataStructuresAssignment.Tests;

internal class TreeTests
{
    internal static void CheckTree(ITestableTree<TestData> tree)
    {
        switch (tree)
        {
            case TestableKdTree<TestData> kd:
                CheckKdTree(kd._tree);
                break;

            case TestableQdTree<TestData> qd:
                CheckQdTree(qd._tree);
                break;

            case TestableQdKdTree<TestData> qdkd:
                CheckQdKdTree(qdkd._tree);
                break;
        }
    }

    internal static void CheckKdTree(KdTree<TestData> tree)
    {
        if (tree.Root is null)
        {
            return;
        }

        Assert.Null(tree.Root.Parent);
        Assert.Equal(0, tree.Root.Dimension);

        var visited = new HashSet<KdNode<TestData>>();
        var stack = new Stack<KdNode<TestData>>();
        stack.Push(tree.Root);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            Assert.True(visited.Add(current), "Cycle!");

            Assert.InRange(current.Dimension, 0, tree.Dimensions - 1);
            Assert.NotEmpty(current.Data);

            var first = current.First;
            Assert.NotNull(first);

            foreach (var data in current.Data)
            {
                for (int dimension = 0; dimension < tree.Dimensions; dimension++)
                {
                    Assert.Equal(0, data.Data.Compare(first, dimension));
                }
            }

            int nextDimension = (current.Dimension + 1) % tree.Dimensions;

            if (current.Left is not null)
            {
                Assert.Same(current, current.Left.Parent);
                Assert.Equal(nextDimension, current.Left.Dimension);

                stack.Push(current.Left);
            }

            if (current.Right is not null)
            {
                Assert.Same(current, current.Right.Parent);
                Assert.Equal(nextDimension, current.Right.Dimension);

                stack.Push(current.Right);
            }

            var node = current;

            while (node.Parent is not null)
            {
                var parent = node.Parent;
                var compare = first.Compare(parent.First!, parent.Dimension);

                if (parent.Left == node)
                {
                    Assert.True(compare <= 0, "Invalid left!");
                }
                else if (parent.Right == node)
                {
                    Assert.True(compare > 0, "Invalid right!");
                }
                else
                {
                    Assert.Fail("Invalid!");
                }

                node = parent;
            }
        }
    }

    internal static void CheckQdTree(QdTree<TestData, DoubleBoundary> tree)
    {
        if (tree.Root is null)
        {
            return;
        }

        Assert.Null(tree.Root.Parent);

        Assert.Equal(0, tree.Root.MinX.Compare(tree.MinX));
        Assert.Equal(0, tree.Root.MinY.Compare(tree.MinY));
        Assert.Equal(0, tree.Root.MaxX.Compare(tree.MaxX));
        Assert.Equal(0, tree.Root.MaxY.Compare(tree.MaxY));

        var visited = new HashSet<QdNode<TestData, DoubleBoundary>>();
        var stack = new Stack<(QdNode<TestData, DoubleBoundary> Node, int Depth)>();
        stack.Push((tree.Root, 0));

        while (stack.Count > 0)
        {
            var (current, depth) = stack.Pop();

            Assert.True(visited.Add(current), "Cycle!");
            Assert.True(depth <= tree.MaxDepth, "Invalid depth!");

            var centerX = current.MinX.Middle(current.MaxX);
            var centerY = current.MinY.Middle(current.MaxY);

            if (current.HasChildren)
            {
                Assert.Empty(current.Data);
            }
            else
            {
                Assert.NotEmpty(current.Data);

                var first = current.First;
                Assert.NotNull(first);

                foreach (var data in current.Data)
                {
                    Assert.True(data.Data.X >= current.MinX.Value, "Invalid X!");
                    Assert.True(data.Data.X <= current.MaxX.Value, "Invalid X!");
                    Assert.True(data.Data.Y >= current.MinY.Value, "Invalid Y!");
                    Assert.True(data.Data.Y <= current.MaxY.Value, "Invalid Y!");

                    var node = current;

                    while (node.Parent is not null)
                    {
                        var parent = node.Parent;
                        var parentCenterX = parent.MinX.Middle(parent.MaxX);
                        var parentCenterY = parent.MinY.Middle(parent.MaxY);

                        if (parent.UpperLeft == node)
                        {
                            Assert.True(data.Data.X <= parentCenterX.Value, "Invalid upper left!");
                            Assert.True(data.Data.Y > parentCenterY.Value, "Invalid upper left!");
                        }
                        else if (parent.UpperRight == node)
                        {
                            Assert.True(data.Data.X > parentCenterX.Value, "Invalid upper right!");
                            Assert.True(data.Data.Y > parentCenterY.Value, "Invalid upper right!");
                        }
                        else if (parent.LowerLeft == node)
                        {
                            Assert.True(data.Data.X <= parentCenterX.Value, "Invalid lower left!");
                            Assert.True(data.Data.Y <= parentCenterY.Value, "Invalid lower left!");
                        }
                        else if (parent.LowerRight == node)
                        {
                            Assert.True(data.Data.X > parentCenterX.Value, "Invalid lower right!");
                            Assert.True(data.Data.Y <= parentCenterY.Value, "Invalid lower right!");
                        }
                        else
                        {
                            Assert.Fail("Invalid parent!");
                        }

                        node = parent;
                    }
                }
            }

            if (current.UpperLeft is not null)
            {
                Assert.Same(current, current.UpperLeft.Parent);
                Assert.Equal(0, current.UpperLeft.MinX.Compare(current.MinX));
                Assert.Equal(0, current.UpperLeft.MinY.Compare(centerY));
                Assert.Equal(0, current.UpperLeft.MaxX.Compare(centerX));
                Assert.Equal(0, current.UpperLeft.MaxY.Compare(current.MaxY));

                stack.Push((current.UpperLeft, depth + 1));
            }

            if (current.UpperRight is not null)
            {
                Assert.Same(current, current.UpperRight.Parent);
                Assert.Equal(0, current.UpperRight.MinX.Compare(centerX));
                Assert.Equal(0, current.UpperRight.MinY.Compare(centerY));
                Assert.Equal(0, current.UpperRight.MaxX.Compare(current.MaxX));
                Assert.Equal(0, current.UpperRight.MaxY.Compare(current.MaxY));

                stack.Push((current.UpperRight, depth + 1));
            }

            if (current.LowerLeft is not null)
            {
                Assert.Same(current, current.LowerLeft.Parent);
                Assert.Equal(0, current.LowerLeft.MinX.Compare(current.MinX));
                Assert.Equal(0, current.LowerLeft.MinY.Compare(current.MinY));
                Assert.Equal(0, current.LowerLeft.MaxX.Compare(centerX));
                Assert.Equal(0, current.LowerLeft.MaxY.Compare(centerY));

                stack.Push((current.LowerLeft, depth + 1));
            }

            if (current.LowerRight is not null)
            {
                Assert.Same(current, current.LowerRight.Parent);
                Assert.Equal(0, current.LowerRight.MinX.Compare(centerX));
                Assert.Equal(0, current.LowerRight.MinY.Compare(current.MinY));
                Assert.Equal(0, current.LowerRight.MaxX.Compare(current.MaxX));
                Assert.Equal(0, current.LowerRight.MaxY.Compare(centerY));

                stack.Push((current.LowerRight, depth + 1));
            }
        }
    }

    internal static void CheckQdKdTree(QdKdTree<TestData, DoubleBoundary> tree)
    {
        if (tree.Root is null)
        {
            return;
        }

        Assert.Null(tree.Root.Parent);

        Assert.Equal(0, tree.Root.MinX.Compare(tree.MinX));
        Assert.Equal(0, tree.Root.MinY.Compare(tree.MinY));
        Assert.Equal(0, tree.Root.MaxX.Compare(tree.MaxX));
        Assert.Equal(0, tree.Root.MaxY.Compare(tree.MaxY));

        var visited = new HashSet<QdKdNode<TestData, DoubleBoundary>>();
        var stack = new Stack<(QdKdNode<TestData, DoubleBoundary> Node, int Depth)>();
        stack.Push((tree.Root, 0));

        while (stack.Count > 0)
        {
            var (current, depth) = stack.Pop();

            Assert.True(visited.Add(current), "Cycle!");
            Assert.True(depth <= tree.MaxDepth, "Invalid depth!");

            var centerX = current.MinX.Middle(current.MaxX);
            var centerY = current.MinY.Middle(current.MaxY);

            Assert.Equal(2, current.Data.Dimensions);
            CheckKdTree(current.Data);

            if (current.HasChildren)
            {
                Assert.Null(current.Data.Root);
            }
            else
            {
                Assert.NotNull(current.Data.Root);

                var kdStack = new Stack<KdNode<TestData>>();
                kdStack.Push(current.Data.Root);

                while (kdStack.Count > 0)
                {
                    var kdCurrent = kdStack.Pop();

                    foreach (var data in kdCurrent.Data)
                    {
                        Assert.True(data.Data.X >= current.MinX.Value, "Invalid X!");
                        Assert.True(data.Data.X <= current.MaxX.Value, "Invalid X!");
                        Assert.True(data.Data.Y >= current.MinY.Value, "Invalid Y!");
                        Assert.True(data.Data.Y <= current.MaxY.Value, "Invalid Y!");

                        var node = current;

                        while (node.Parent is not null)
                        {
                            var parent = node.Parent;
                            var parentCenterX = parent.MinX.Middle(parent.MaxX);
                            var parentCenterY = parent.MinY.Middle(parent.MaxY);

                            if (parent.UpperLeft == node)
                            {
                                Assert.True(data.Data.X <= parentCenterX.Value, "Invalid upper left!");
                                Assert.True(data.Data.Y > parentCenterY.Value, "Invalid upper left!");
                            }
                            else if (parent.UpperRight == node)
                            {
                                Assert.True(data.Data.X > parentCenterX.Value, "Invalid upper right!");
                                Assert.True(data.Data.Y > parentCenterY.Value, "Invalid upper right!");
                            }
                            else if (parent.LowerLeft == node)
                            {
                                Assert.True(data.Data.X <= parentCenterX.Value, "Invalid lower left!");
                                Assert.True(data.Data.Y <= parentCenterY.Value, "Invalid lower left!");
                            }
                            else if (parent.LowerRight == node)
                            {
                                Assert.True(data.Data.X > parentCenterX.Value, "Invalid lower right!");
                                Assert.True(data.Data.Y <= parentCenterY.Value, "Invalid lower right!");
                            }
                            else
                            {
                                Assert.Fail("Invalid parent!");
                            }

                            node = parent;
                        }
                    }

                    if (kdCurrent.Left is not null)
                    {
                        kdStack.Push(kdCurrent.Left);
                    }

                    if (kdCurrent.Right is not null)
                    {
                        kdStack.Push(kdCurrent.Right);
                    }
                }
            }

            if (current.UpperLeft is not null)
            {
                Assert.Same(current, current.UpperLeft.Parent);
                Assert.Equal(0, current.UpperLeft.MinX.Compare(current.MinX));
                Assert.Equal(0, current.UpperLeft.MinY.Compare(centerY));
                Assert.Equal(0, current.UpperLeft.MaxX.Compare(centerX));
                Assert.Equal(0, current.UpperLeft.MaxY.Compare(current.MaxY));

                stack.Push((current.UpperLeft, depth + 1));
            }

            if (current.UpperRight is not null)
            {
                Assert.Same(current, current.UpperRight.Parent);
                Assert.Equal(0, current.UpperRight.MinX.Compare(centerX));
                Assert.Equal(0, current.UpperRight.MinY.Compare(centerY));
                Assert.Equal(0, current.UpperRight.MaxX.Compare(current.MaxX));
                Assert.Equal(0, current.UpperRight.MaxY.Compare(current.MaxY));

                stack.Push((current.UpperRight, depth + 1));
            }

            if (current.LowerLeft is not null)
            {
                Assert.Same(current, current.LowerLeft.Parent);
                Assert.Equal(0, current.LowerLeft.MinX.Compare(current.MinX));
                Assert.Equal(0, current.LowerLeft.MinY.Compare(current.MinY));
                Assert.Equal(0, current.LowerLeft.MaxX.Compare(centerX));
                Assert.Equal(0, current.LowerLeft.MaxY.Compare(centerY));

                stack.Push((current.LowerLeft, depth + 1));
            }

            if (current.LowerRight is not null)
            {
                Assert.Same(current, current.LowerRight.Parent);
                Assert.Equal(0, current.LowerRight.MinX.Compare(centerX));
                Assert.Equal(0, current.LowerRight.MinY.Compare(current.MinY));
                Assert.Equal(0, current.LowerRight.MaxX.Compare(current.MaxX));
                Assert.Equal(0, current.LowerRight.MaxY.Compare(centerY));

                stack.Push((current.LowerRight, depth + 1));
            }
        }
    }
}