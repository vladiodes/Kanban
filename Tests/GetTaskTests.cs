using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using NUnit.Framework;
using Moq;
using IntroSE.Kanban.Backend.BusinessLayer.TaskPackage;
using IntroSE.Kanban.Backend.BusinessLayer.ColumnPackage;
using System;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System.Collections.Generic;

namespace Tests
{
	/// <summary>
	/// Testing the function getTask - which gets a task to perform some kind of a change on the task.
	/// </summary>
	[TestFixture]
	class GetTaskTests
	{
		Mock<BoardDAL> boardDalMock;
		Mock<Column> columnMock1;
		Mock<Column> columnMock2;
		Mock<Ttask> taskMock;
		Board board;

		/// <summary>
		/// base setup to test Board for Testing.The board will start with 2 columns.
		/// </summary>
		[SetUp]
		public void Base_Setup()
		{
			taskMock = new Mock<Ttask>();
			columnMock1 = new Mock<Column>();
			boardDalMock = new Mock<BoardDAL>();
			columnMock2 = new Mock<Column>();
			List<int> AccessID = new List<int>();//Users that are related to the current board.
			AccessID.Add(1);
			AccessID.Add(2);
			AccessID.Add(3);

			board = new Board(boardDalMock.Object, AccessID);
			board.ColumnsByOrder.Add(columnMock1.Object);
			board.ColumnsByOrder.Add(columnMock2.Object);
		}

		/// <summary>
		/// a test that checks the method while it assumes that everything is logical.
		/// </summary>
		[Test]
		[TestCase(3,0)]
		public void GetTask_AssumeGoodInputs_ReturnNonNulTask(int taskid,int columnOrdinal)
		{
			//Arrange
			columnMock1.SetupGet(m => m.Tasks).Returns(() => new List<Ttask> { taskMock.Object });
			columnMock1.Setup(m => m.getTask(taskid)).Returns(taskMock.Object);

			//Act
			Ttask output = board.GetTask(taskid, columnOrdinal);

			//Assert
			Assert.AreSame(taskMock.Object, output,"Should return a task");
		}

		/// <summary>
		/// tests the method while it assumes it was given a non-valid taskid.
		/// </summary>
		[Test]
		public void GetTask_BadTaskId_ReturnException()
		{
			//Arrange
			columnMock1.Setup(m => m.Tasks).Returns(() => new List<Ttask> { taskMock.Object });
			columnMock1.Setup(m => m.getTask(It.IsAny<int>())).Returns(() => null);

			//Act

			//Assert
			var ExceptionMessage = Assert.Throws<ApplicationException>(() => board.GetTask(3, 0));
			Assert.That(ExceptionMessage.Message, Is.EqualTo("No such task in this column"));
		}

		/// <summary>
		/// tests the method when columnordinal is nor in range. 
		/// </summary>
		[Test]
		[TestCase(-1)]
		[TestCase(5)]
		public void GetTask_ColumnOrdinalNotInRange_ReturnException(int columnOrdinal)
		{
			//Arrange

			//Act

			//Assert
			var ExceptionMessage = Assert.Throws<ApplicationException>(() => board.GetTask(3, columnOrdinal));
			Assert.That(ExceptionMessage.Message, Is.EqualTo("This column isn't within the range"));
		}


		/// <summary>
		/// execute the method when the colums dosent have tasks.
		/// </summary>
		[Test]
		public void GetTask_NoTasksIncolumn_ReturnException()
		{
			//Arrange
			columnMock1.Setup(m => m.Tasks).Returns(() => new List<Ttask> { }); //has not tasks inside him

			//Act

			//Assert
			var ExceptionMessage = Assert.Throws<ApplicationException>(() => board.GetTask(3, 0));
			Assert.That(ExceptionMessage.Message, Is.EqualTo("You don't have any tasks in this column"));
		}


		/// <summary>
		/// tests method when trying to take the rightmost column
		/// </summary>
		[Test]
		public void GetTask_RightMostColumn_ReturnException()
		{
			//Arrange
			columnMock1.Setup(m => m.Tasks).Returns(() => new List<Ttask> { taskMock.Object });

			//Act

			//Assert
			var ExceptionMessage = Assert.Throws<ApplicationException>(() => board.GetTask(3, 1));
			Assert.That(ExceptionMessage.Message, Is.EqualTo("You can't make changes to a task in the rightmost column"));
		}
	}
}
