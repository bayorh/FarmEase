
using Domain.Contracts;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;



namespace Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordHasher _hasher;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
       
        
        public AuthService(IAsyncRepository<User> userRepository,IJwtProvider jwtProvider, IPasswordHasher 
            passwordHasher,IEmailService emailservice,IConfiguration config, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _hasher = passwordHasher;
            _emailService = emailservice;
            _config = config;
            _logger = logger;
        }
        public async Task<string> Authenticate(string usernameOrEmail, string password)
        {
            var user = await _userRepository
                .GetSingleAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
             if(user == null)
                 throw new NotFoundException($"Usesr with {usernameOrEmail} not found");
            if (!user.VerifyPassword(password, _hasher))
                throw new BadRequestException("Invalid Password");
            return _jwtProvider.GenerateToken(user);
        }

        public async Task Register(string username, string email, string password)
        {
            var _user = await _userRepository.GetSingleAsync(u => u.Username == username || u.Email == email);
            if (_user != null) throw new AlreadyExistException("user already exist");
            _user = User.Create(username, email, password, _hasher);
            _user.InitiatedBy = email;
            await _userRepository.AddAsync(_user);
            await _userRepository.SaveCnangesAsync();
        }

        public async Task GetResetPasswordTokenbyMail(string email)
        {
            var user = await _userRepository.GetSingleAsync(u => u.Email == email);
            if (user == null)
                throw new NotFoundException($"user with {email} not found");
            var token = _jwtProvider.GeneratePasswordResetToken(user); 
            _logger.LogInformation(token); //to be modified
            var resetLink = $"{_config["Frontend:ResetPasswordUrl"]}?token={token}";
            await _emailService.SendAsync(user.Email, "Reset Your Password", $"Click to reset: {resetLink}");
        }
        public async Task UpdatePassword(string password,string email, string resetToken)
        {
            var user = await _userRepository.GetSingleAsync(u => u.Email == email);
            if (user == null) throw new NotFoundException($"User with Email: {email} not found");
            
            //validate reset token
            var validationResult = await _jwtProvider.ValidateResetToken(resetToken);
           if (!validationResult) throw new TokenValidationException("Inavalid or Expired validation token."); 
            var _user =  user.UpdatePassword(password, _hasher);
            await _userRepository.UpdateAsync(_user);
            await _userRepository.SaveCnangesAsync();
        }

        public async Task<User> GetUserbyEmail(string email)
        {
            var user = await _userRepository.GetSingleAsync(u => u.Email == email);
            if (user == null) throw new NotFoundException($"User with Email: {email} not found");
            return user;
        }

       
    }
}
