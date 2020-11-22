using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using NUnit.Framework;
using Moq;
using IntroSE.Kanban.Backend.BusinessLayer.ColumnPackage;
using System;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.BusinessLayer.TaskPackage;

namespace Tests
{
    /// <summary>
    /// In this test we check the correctness of the advance task functionality - without the dependent on the get task function,
    /// as we have already checked it independently, and therefore we can assume that the get task will throw an exception in case we want to advance a task
    /// from the rightmost column, or doesn't exit, etc.. (any case that doesn't meet the requirements of updating task's state).
    /// </summary>
    [TestFixture]
    class AdvanceTaskTests
    {
        Mock<BoardDAL> boardDalMock;
        Mock<Column> column0;
        Mock<Column> column1;
        Mock<Column> column2;
        Mock<Ttask> task1;

        Board board;

        [SetUp]
        public void Setup()
        {
            column0 = new Mock<Column>();
            column1 = new Mock<Column>();
            column2 = new Mock<Column>();
            task1 = new Mock<Ttask>();

            boardDalMock = new Mock<BoardDAL>();
            boardDalMock.SetupGet(m => m.CreatorID).Returns(1);
            boardDalMock.SetupGet(m => m.Id).Returns(1);

            List<int> AccessID = new List<int>();//Users that are related to the current board.
            AccessID.Add(1);
            AccessID.Add(2);
            AccessID.Add(3);

            board = new Board(boardDalMock.Object, AccessID);
            //inserting columns to board
            board.ColumnsByOrder.Add(column0.Object);
            board.ColumnsByOrder.Add(column1.Object);
            board.ColumnsByOrder.Add(column2.Object);

            board.ColumnsByNames["Column0"] = column0.Object;
            board.ColumnsByNames["Column1"] = column1.Object;
            board.ColumnsByNames["Column2"] = column2.Object;
        }

        //in all of the following tests - we assume that whoever wants to advance the task is the actual assignee of the task.

        [Test]
        public void AdvanceTask_NextColumnFull_Fail()
        {
            string output = "";
            //complete random variables - don't really matter to the purpose of this test
            int assigneeId = 3;
            int RandomtaskId = 5;
            //Arange

            //will not throw any exceptions
            task1.Setup(t => t.isAssignee(assigneeId));

            //moking that the task is in the first column
            column0.SetupGet(c => c.Tasks).Returns(() => new List<Ttask>() { task1.Object });

            //will return the mocked task1.
            column0.Setup(c => c.getTask(RandomtaskId)).Returns(task1.Object);

            //no place to hold the advanced task in the next column
            column1.Setup(c => c.checkLimit()).Returns(false);


            //Act
            try
            {
                board.AdvanceTask(0, RandomtaskId, assigneeId);
            }
            catch(Exception e)
            {
                output = e.Message;
            }

            //Assert
            Assert.AreEqual("You can't advance to the next column because you've reached to limit in this column", output, "Sholdn't make the advance but rather throw an apropriate exception");
        }

        //advancing a task from column0 to column1 successfully
        [Test]
        public void AdvanceTask_Success()
        {
            //complete random variables - don't really matter to the purpose of this test
            int assigneeId = 3;
            int RandomtaskId = 5;

            //Arange

            //will not throw any exceptions
            task1.Setup(t => t.isAssignee(assigneeId));

            //moking that the task is in the first column
            column0.SetupGet(c => c.Tasks).Returns(() => new List<Ttask>() { task1.Object });

            //will return the mocked task1.
            column0.Setup(c => c.getTask(RandomtaskId)).Returns(task1.Object);

            //There's enough space for it to hold the advanced task
            column1.Setup(c => c.checkLimit()).Returns(true);

            //checking that the functions of delete and add of columns were called exactly once
            column0.Setup(c => c.DeleteTask(task1.Object)).Verifiable();
            column1.Setup(c => c.AddTask(task1.Object)).Verifiable();

            //Act
            board.AdvanceTask(0, RandomtaskId, assigneeId);

            //Assert
            column0.Verify(c => c.DeleteTask(task1.Object), Times.Once);
            column1.Verify(c => c.AddTask(task1.Object), Times.Once);
        }


    }
}
