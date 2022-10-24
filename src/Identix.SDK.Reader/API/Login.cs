namespace Identix.SDK.API
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }

        //POST_HANDLERS:  http://10.0.0.59/config/login
        //GET_UPDATE_CONFIGS/LOGIN: http://{{MINIPAD_IP}}:{{MINIPAD_PORT}}/config/login
    }
}
