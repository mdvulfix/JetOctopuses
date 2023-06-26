namespace Core
{
    public class User
    {
        public string Name { get; private set; }
        public string Age { get; private set; }

        public string Login { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }

        public string Role { get; private set; }

    }

    public enum UserAction
    {
        None,
        Login,
        MenuMain,
        MenuOptions,
        MenuPlay,
        MenuExit,
        LevelPlay,
        LevelPause,
        LevelExit,
        ResultExit
    }

}