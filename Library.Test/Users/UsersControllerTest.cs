namespace Library.Test.Users
{
    public class UsersControllerTest
    {
        private static readonly IDataRepository<UsersDto> _dataRepository = new UsersMockDataRepository();
        private readonly UsersAPIController _usersAPIController = new UsersAPIController(_dataRepository);
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllAsync_Test_Pass()
        {
            //  ARRANGE
            int expectedCount = 2;
            //  ACT
            ActionResult<IEnumerable<UsersDto>> usersFromAPI = await _usersAPIController.GetUsers();
            IEnumerable<UsersDto> users = (IEnumerable<UsersDto>)((Microsoft.AspNetCore.Mvc.ObjectResult)usersFromAPI.Result).Value;
            //  ASSERT
            Assert.True(users != null);
            Assert.That(users.Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task GetById_Test_Pass()
        {
            //  ARRANGE
            int id = 1;
            //  ACT
            var usersFromAPI = await _usersAPIController.GetUser(id);
            UsersDto user = (UsersDto)((Microsoft.AspNetCore.Mvc.ObjectResult)usersFromAPI.Result).Value;
            //  ASSERT
            Assert.True(user != null);
            Assert.That(user.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task GetById_Test_Fail()
        {
            //  ARRANGE
            int id = 3;
            //  ACT
            ActionResult<UsersDto> usersFromAPI = await _usersAPIController.GetUser(id);
            //  ASSERT
            Assert.IsInstanceOf<NotFoundResult>(usersFromAPI.Result);
        }

        [Test]
        public async Task InsertAsync_Test_Pass()
        {
            //  ARRANGE
            UsersDto modelToInsert = new UsersDto
            {
                Name = "test3",
                Surname = "test3",
                Email = "test3@test3.test3",
                Department = "test3",
                Id = 3,
                CreatedOn = DateTime.Now,
                SockColour = "orange"
            };
            //  ACT
            await _usersAPIController.InsertUser(modelToInsert);
            var usersFromAPI = await _usersAPIController.GetUser(modelToInsert.Id);
            UsersDto user = (UsersDto)((Microsoft.AspNetCore.Mvc.ObjectResult)usersFromAPI.Result).Value;
            //  ASSERT
            Assert.True(user != null);
            Assert.That(user.Id, Is.EqualTo(modelToInsert.Id));
        }
    }
}
