using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using NUnit.Framework;
using Moq;
using IntroSE.Kanban.Backend.BusinessLayer.ColumnPackage;
using System;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System.Collections.Generic;

namespace Tests
{
    /// <summary>
    /// In these tests we check the functionality of the move column right/left functions.
    /// We assume that the methods are exceuted by the creator of the board - as the function isCreator() throws an appropriate exception otherwise.
    /// </summary>
    [TestFixture]
    class MoveColumnTests
    {
        int randomSetter; //places the values from mocked setters to this variable
        Mock<BoardDAL> boardDalMock;
        Mock<Column> column0;
        Mock<Column> column1;
        Mock<Column> column2;

        Board board;

        [SetUp]
        public void Setup()
        {
            column0 = new Mock<Column>();
            column1 = new Mock<Column>();
            column2 = new Mock<Column>();

            boardDalMock = new Mock<BoardDAL>();
            boardDalMock.SetupGet(m => m.CreatorID).Returns(1);
            boardDalMock.SetupGet(m => m.Id).Returns(1);
            column0.SetupSet(c => c.ColumnOrdinal = It.IsAny<int>()).Callback<int>(value =>randomSetter=value);
            column1.SetupSet(c => c.ColumnOrdinal = It.IsAny<int>()).Callback<int>(value => randomSetter = value);
            column2.SetupSet(c => c.ColumnOrdinal = It.IsAny<int>()).Callback<int>(value => randomSetter = value);

            List<int> AccessID = new List<int>();//Users that are related to the current board.
            AccessID.Add(1);
            AccessID.Add(2);
            AccessID.Add(3);

            board = new Board(boardDalMock.Object, AccessID);
            //inserting columns to board
            board.ColumnsByOrder.Add(column0.Object);
            board.ColumnsByOrder.Add(column1.Object);
            board.ColumnsByOrder.Add(column2.Object);
        }

        //in the following tests we assume all movement tests are excecutad by the creator of the board

        [Test]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(2)] //only 3 columns - so 2=rightmost
        [TestCase(50)]
        public void MoveColumnRight_NegativeOrdinal_RightMostOrLarger_Fail(int columnOrdinal)
        {
            string output = "";
            //arange

            //act
            try
            {
                board.MoveColumnRight(columnOrdinal, board.CreatorID);
            }
            catch(Exception e)
            {
                output = e.Message;
            }

            //assert
            Assert.AreEqual("No negative ordinal, or rightmost column can't be moved to the right", output, "Should have thrown an exception with an appropriate message");
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(3)] //rightmost+1=3
        [TestCase(4)]
        public void MoveColumnLeft_NegativeOrZeroOrdinal_LargerRightmost_Fail(int columnOrdinal)
        {
            string output = "";
            //arange

            //act
            try
            {
                board.MoveColumnLeft(columnOrdinal, board.CreatorID);
            }
            catch (Exception e)
            {
                output = e.Message;
            }

            //assert
            Assert.AreEqual("Column doesn't exist or either left most or negative ordinal", output, "Should have thrown an exception with an appropriate message");
        }

        
        [Test]
        [TestCase(0)] //moving first column to right
        [TestCase(1)] //moving second column to right
        public void MoveColumnRight_Success(int columnOrdinal)
        {

            //arange
            Column original = board.ColumnsByOrder[columnOrdinal];
            Column right = board.ColumnsByOrder[columnOrdinal + 1];

            //act
            board.MoveColumnRight(columnOrdinal, board.CreatorID);

            //assert
            //Checking if the two columns were swapped
            Assert.AreEqual(original, board.ColumnsByOrder[columnOrdinal + 1], "Shold have made the swap");
            Assert.AreEqual(right, board.ColumnsByOrder[columnOrdinal], "Shold have made the swap");

        }

        [Test]
        [TestCase(1)] //moving middle column left
        [TestCase(2)] //moving rightmost column left
        public void MoveColumnLeft_Success(int columnOrdinal)
        {    
            //arange
            Column original = board.ColumnsByOrder[columnOrdinal];
            Column left = board.ColumnsByOrder[columnOrdinal - 1];

            //act
            board.MoveColumnLeft(columnOrdinal, board.CreatorID);

            //assert
            //Checking if the two columns were swapped
            Assert.AreEqual(original, board.ColumnsByOrder[columnOrdinal - 1], "Shold have made the swap");
            Assert.AreEqual(left, board.ColumnsByOrder[columnOrdinal], "Shold have made the swap");
        }


    }
}
