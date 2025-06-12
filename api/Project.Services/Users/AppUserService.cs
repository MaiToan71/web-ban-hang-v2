using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Enums;
using Project.Services.Users.Interfaces;
using Project.Utilities.Exceptions;
using Project.Utilities.Helper;
using Project.ViewModel.Usermanagers.Permissions;
using Project.ViewModel.Usermanagers.Roles;
using Project.ViewModel.Usermanagers.Users;
using Project.ViewModels;
using Project.ViewModels.TimeShifts;
using Project.ViewModels.TimeShiftTypes;
using Project.ViewModels.Usermanagers.Users;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.Usermager.Services
{
    public class ExternalAuthDto
    {
        public string? Provider { get; set; }
        public string? IdToken { get; set; }
    }
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ProductDbContext _context;
        private readonly IConfigurationSection _goolgeSettings;
        public AppUserService(UserManager<AppUser> userManager, ProductDbContext context, SignInManager<AppUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _config = config;
            _goolgeSettings = _config.GetSection("GoogleAuthSettings");
        }
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _goolgeSettings.GetSection("clientId").Value }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }
        public async Task<string> AuthencateGoogle(LoginGoogle request)
        {

            var externalAuth = new ExternalAuthDto
            {
                Provider = "GOOGLE",
                IdToken = request.IdToken,
            };
            var payload = await this.VerifyGoogleToken(externalAuth);
            if (payload == null)
            {
                throw new ProductException($"Invalid External Authentication.");
            };

            string defaultPass = "Google@070197";
            var checkUser = _context.AppUsers.Where(m => m.UserName == request.Email.Trim()).FirstOrDefault();


            if (checkUser == null)
            {
                var register = new UserCreateRequest()
                {

                    Email = request.Email.Trim(),
                    FullName = request.Name.Trim(),
                    UserName = request.Email.Trim(),
                    IsCustomer = true,
                    GoogleId = request.GoogleId.Trim(),
                    IdToken = request.IdToken.Trim(),
                    Password = defaultPass
                }
                    ;
                await this.Register(register);
            }
            else
            {

                if (request.GoogleId.Trim() != checkUser.GoogleId)
                {
                    throw new ProductException($"GoogleId not verify");
                }
                var update = new UserUpdateRequest()
                {
                    Email = request.Email.Trim(),
                    FullName = request.Name.Trim(),
                    UserId = checkUser.Id,
                    IdToken = request.IdToken.Trim(),
                };
                await this.Update(update);
            }
            // login 
            var requestLogin = new LoginRequest()
            {
                Password = defaultPass,
                Username = request.Email,
            };
            var token = await this.AuthencateGoogle(requestLogin);

            return token;


        }
        public async Task<string> AuthencateGoogle(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null) return null;
            var userContext = await _context.AppUsers.FirstOrDefaultAsync(m => m.UserName == request.Username);
            if (userContext == null) return null;
            if (userContext.IsDelete)
            {
                return null;
            }
            //    var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            //if (!result.Succeeded)
            //{
            //return null;
            //}
            //get roles in user
            List<int> roleIds = new List<int>();
            var listRoles = await _context.AppUsermangersRoles.Where(r => r.UserId == user.Id).ToListAsync();
            foreach (var role in listRoles)
            {
                roleIds.Add(role.RoleId);
            }

            //get permission

            var permissionName = new List<string>();
            var listPers = await _context.RolePermissions.Include(m => m.Permission).Where(p => roleIds.Contains(p.RoleId)).ToListAsync();
            foreach (var per in listPers)
            {
                permissionName.Add(per.Permission.NormalizedName);
            }

            var anonymous = TokenUserType.IsManage;
            if (user.IsCustomer == true)
            {
                anonymous = TokenUserType.IsClient;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.Id.ToString()),
                new Claim(ClaimTypes.GivenName,user.FullName),
                new Claim(ClaimTypes.Anonymous,anonymous.ToString()),
               new Claim(ClaimTypes.Role, string.Join(",", permissionName)),

            };

            var configKey = _config["JWTs:Key"];
            var issuer = _config["JWTs:Issuer"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public async Task<ViewDataLoginViewModel> Authencate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null) throw new ProductException($"Không tồn tại tài khoản");
            var userContext = await _context.AppUsers.FirstOrDefaultAsync(m => m.UserName == request.Username);
            if (userContext == null) throw new ProductException($"Không tồn tại tài khoản");
            if (userContext.IsDelete)
            {
                throw new ProductException($"Tài khoản đã bị xóa. Vui lòng liên hệ quản trị viên");
            }
            if (userContext.IsActive == false)
            {
                throw new ProductException($"Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên");
            }
            if (userContext.Workflow != Workflow.COMPLETED)
            {
                throw new ProductException($"Tài khoản chưa được duyệt. Vui lòng liên hệ quản trị viên ");
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);

            if (!result.Succeeded)
            {
                throw new ProductException($"Đăng nhập sai mật khẩu");
            }
            //get roles in user
            //List<int> roleIds = new List<int>();
            //var listRoles = await _context.AppUsermangersRoles.Where(r => r.UserId == user.Id).ToListAsync();
            //foreach (var role in listRoles)
            //{
            //    roleIds.Add(role.RoleId);
            //}

            ////get permission

            //var permissionName = new List<string>();
            //if (roleIds.Count > 0)
            //{
            //    var listPers = await _context.RolePermissions.Include(m => m.Permission).Where(p => roleIds.Contains(p.RoleId)).Where(m => m.Permission.IsDeleted == false).ToListAsync();
            //    foreach (var per in listPers)
            //    {
            //        permissionName.Add(per.Permission.NormalizedName);
            //    }
            //}


            var anonymous = TokenUserType.IsManage;
            if (user.IsCustomer == true)
            {
                anonymous = TokenUserType.IsClient;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Id.ToString()),
                new Claim(ClaimTypes.GivenName,user.FullName),
                new Claim(ClaimTypes.Anonymous,anonymous.ToString()),

            };

            var configKey = _config["JWTs:Key"];
            var issuer = _config["JWTs:Issuer"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var res = new ViewDataLoginViewModel()
            {
                Status = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Data = await this.GetUserInfo(user.Id),
            };
            return res;

        }


        public async Task<int> DeleteAppUserRoles(Guid userId)
        {
            var items = _context.AppUsermangersRoles.Where(m => m.UserId == userId);
            _context.AppUsermangersRoles.RemoveRange(items);
            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> AddAppUserRoles(List<int> roleIds, Guid userId)
        {
            List<AppUsermangersRole> items = new List<AppUsermangersRole>();
            foreach (var i in roleIds)
            {
                var item = new AppUsermangersRole
                {
                    RoleId = i,
                    UserId = userId
                };
                items.Add(item);
            }
            await _context.AppUsermangersRoles.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<dynamic> Register(UserCreateRequest request)
        {

            var checkUser = _context.AppUsers.Where(m => m.UserName == request.UserName);
            if (checkUser.Count() > 0)
            {
                throw new ProductException($"Đã tồn tại tài khoản. Vui lòng nhập thông tin khác");
            }
            var checkUser2 = _context.AppUsers.Where(m => m.UserName == request.UserName).Where(m => m.IsDelete == true);
            if (checkUser.Count() > 0)
            {
                throw new ProductException($"Đã tồn tại tài khoản. Tài khoản bạn đã bị khóa");
            }
            var user = new AppUser()
            {

                CreatedTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                Email = request.Email.Trim(),
                FullName = request.FullName.Trim(),
                UserName = request.UserName.Trim(),
                UserType = request.UserType,
                Workflow = request.Workflow,
                IsCustomer = (bool)request.IsCustomer,

            };
            if (request.UserName != null)
            {
                user.PhoneNumber = request.UserName.Trim();
            }
            if (request.GoogleId != null)
            {
                user.GoogleId = request.GoogleId;

            }
            if (request.IdToken != null)
            {
                user.IdGoogleToken = request.IdToken;
                user.IsGoogle = true;
            }
            var result = await _userManager.CreateAsync(user, request.Password);

            if (request.RoleIds != null)
            {
                if (request.RoleIds.Count() > 0)
                    if (result.Succeeded)
                    {
                        try
                        {
                            await this.AddAppUserRoles(request.RoleIds, user.Id);
                        }
                        catch (Exception e)
                        {
                            throw new ProductException($"Đã tồn tại tài khoản. Tài khoản bạn đã bị khóa");
                        }
                        return result;
                    }
            }
            return result;
        }
        public async Task<bool> UpdateWorkflow(UserUpdateworkdlowRequest request)
        {
            var user = await _context.AppUsers.FindAsync(request.UserId);

            if (user != null)
            {
                user.Workflow = request.Workflow;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> Update(UserUpdateRequest request)
        {
            var user = await _context.AppUsers.FindAsync(request.UserId);
            user.Email = request.Email;
            user.FullName = request.FullName;
            user.ModifiedTime = DateTime.Now;
            user.IsActive = request.IsActive;
            user.UserType = request.UserType;
            user.Workflow = request.Workflow;
            if (request.PhoneNumber != null)
            {
                user.PhoneNumber = request.PhoneNumber;

            }


            if (request.IdToken != null)
            {
                user.IdGoogleToken = request.IdToken;
                user.IsGoogle = true;
            }
            await _context.SaveChangesAsync();
            if (request.RoleIds != null)
            {

                await this.DeleteAppUserRoles(user.Id);
                await this.AddAppUserRoles(request.RoleIds, user.Id);
            }

            if (request.DepartmentIds != null)
            {
                if (request.DepartmentIds.Count() > 0)
                {
                    await this.AddOrUpdateDepartmentUser(request.DepartmentIds, user.Id);
                }
            }


            return true;
        }
        public async Task<bool> AddOrUpdateDepartmentUser(List<int> DepartmentIds, Guid UserId)
        {
            var items = _context.UserDepartments.Where(m => m.UserId == UserId).ToList();
            if (items.Count() > 0)
            {
                _context.UserDepartments.RemoveRange(items);
                await _context.SaveChangesAsync();
            }
            List<UserDepartment> adds = new List<UserDepartment>();
            foreach (var i in DepartmentIds)
            {
                if (i != 0)
                {
                    var item = new UserDepartment
                    {
                        DepartmentId = i,
                        UserId = UserId
                    };
                    adds.Add(item);
                }

            }
            if (adds.Count > 0)
            {
                _context.UserDepartments.AddRange(adds);
                await _context.SaveChangesAsync();
            }


            return true;

        }

        public UserViewModel GetUserById(Guid userId)
        {
            var user = _context.AppUsers.Select(u => new UserViewModel
            {
                UserId = u.Id,
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                UserName = u.UserName,
                IsActive = u.IsActive,
                Roles = (List<RoleViewModel>)(from r in u.AppUsermangersRoles
                                              select new RoleViewModel
                                              {
                                                  Description = r.Role.Description,
                                                  Id = r.RoleId,
                                                  Name = r.Role.Name,
                                                  NormalizedName = r.Role.NormalizedName,

                                              })

            }).FirstOrDefault(m => m.UserId == userId);
            return user;
        }
        public async Task<UserViewModel> GetById(Guid userId)
        {
            var user = await _context.AppUsers.Select(u => new UserViewModel
            {
                UserId = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                UserName = u.UserName,
                IsActive = u.IsActive,
                Balance = u.Balance,
                Roles = (List<RoleViewModel>)(from r in u.AppUsermangersRoles
                                              select new RoleViewModel
                                              {
                                                  Description = r.Role.Description,
                                                  Id = r.RoleId,
                                                  Name = r.Role.Name,
                                                  NormalizedName = r.Role.NormalizedName,

                                              })

            }).FirstOrDefaultAsync(m => m.UserId == userId);
            return user;
        }

        public async Task<PagedResult<UserViewModel>> GetAllPaging(GetUserPagingRequest request)
        {
            //1. query
            var query = _context.AppUsers.Where(m => m.IsDelete == false)
                .AsQueryable();

            //2. filter
            if (request.Username != null)
            {
                query = query.Where(m => m.UserName.Contains(request.Username));
            }

            if (request.IsCustomer != null)
            {
                query = query.Where(m => m.IsCustomer == request.IsCustomer);
            }
            if (request.Workflow != null)
            {
                query = query.Where(m => m.Workflow == request.Workflow);
            }
            if (request.UserType != null)
            {
                query = query.Where(m => m.UserType == request.UserType);
            }
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(m => m.UserName.Contains(request.Search) ||
                m.FullName.Contains(request.Search) || m.Email.Contains(request.Search));
            }

            //3. paging
            int totalRow = query.Count();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
               .Select(u => new UserViewModel
               {
                   Id = u.Id,
                   UserId = u.Id,
                   Email = u.Email,
                   FullName = u.FullName,
                   PhoneNumber = u.PhoneNumber,
                   UserName = u.UserName,
                   Address = u.Address,
                   IsActive = u.IsActive,
                   File = u.FileAccount,
                   Workflow = u.Workflow,
                   UserType = u.UserType,
                   //  CustomerCount= u.Customers.Count(),
                   Roles = (List<RoleViewModel>)(from r in u.AppUsermangersRoles
                                                 select new RoleViewModel
                                                 {
                                                     Description = r.Role.Description,
                                                     Id = r.RoleId,
                                                     Name = r.Role.Name,
                                                     NormalizedName = r.Role.NormalizedName,

                                                 })

               })
                .ToListAsync();
            var userIds = data.Select(m => m.Id).ToList();

            //department
            var UserDepartments = await _context.UserDepartments.Where(m => userIds.Contains(m.UserId)).ToListAsync();
            var Departments = await _context.Departments.Where(m => UserDepartments.Select(m => m.DepartmentId).Contains(m.Id)).ToListAsync();

            foreach (var d in data)
            {
                var checkUserDepartments = UserDepartments.Where(m => m.UserId == d.Id);
                if (checkUserDepartments.Count() > 0)
                {
                    foreach (var p in checkUserDepartments)
                    {
                        var checkDepartments = Departments.Where(m => m.Id == p.DepartmentId);
                        if (checkDepartments.Count() > 0)
                        {
                            d.Departments.AddRange(checkDepartments);
                        }
                    }
                }
            }

            //4 .Select and projecttion
            var pagedResult = new PagedResult<UserViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
            };

            return pagedResult;

        }

        public async Task<int> Delete(Guid id)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
            {
                throw new ProductException($"Cannot find a user with id: {id}");
            }
            user.IsDelete = true;
            await _context.SaveChangesAsync();
            return 1;
        }
        public async Task<int> OpenUser(Guid id)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
            {
                throw new ProductException($"Cannot find a user with id: {id}");
            }
            user.IsDelete = false;
            await _context.SaveChangesAsync();
            return 1;
        }
        public async Task<UserInfoViewModel> GetUserInfo(Guid userId)
        {
            List<int> roleIds = new List<int>();
            var user = await _context.AppUsers.Include(m => m.AppUsermangersRoles).FirstOrDefaultAsync(m => m.Id == userId);
            foreach (var r in user.AppUsermangersRoles)
            {
                roleIds.Add(r.RoleId);
            }
            // var roleMenus = await _context.RoleMenus.Include(m => m.Menu).Where(m => roleIds.Contains(m.RoleId)).ToListAsync();
            var rolePermissions = await _context.RolePermissions.Where(m => roleIds.Contains(m.RoleId)).ToListAsync();
            var listPers = await _context.Permissions.Where(m => m.IsDeleted == false && rolePermissions.Select(r => r.PermissionId).Contains(m.Id)).Select(m => new PermissionViewModel
            {
                Name = m.Name,
                Label = m.Label,
                ParentId = m.ParentId,
                Route = m.Route,
                Component = m.Component,
                Icon = m.Icon,
                Order = m.Order,
                Status = m.Status,
                Hide = m.Hide,
                PermissionType = m.PermissionType,
                Id = m.Id
            }).ToListAsync();
            var obj = new UserInfoViewModel
            {
                CreatedTime = user.CreatedTime,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                // Menus = menus,
                Permissions = listPers,
                Balance = user.Balance,
                Address = user.Address,
                UserId = userId,


            };

            return obj;
        }

        public async Task<bool> ClientUpdate(ClientUpdateRequest request)
        {
            if (request.UserId == null)
            {
                return false;
            }
            var user = await _context.AppUsers.FindAsync(request.UserId);
            if (user == null)
            {
                return false;
            }
            user.Address = request.Address;
            user.FullName = request.FullName;

            user.ModifiedTime = DateTime.Now;
            user.PhoneNumber = request.PhoneNumber;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ResponseViewModel> ChangePassword(UpdatePasswordRequest request)
        {
            var response = new ResponseViewModel();
            if (request.UserId == null)
            {
                response.Status = false;
                response.Message = "Không tìm thấy userId";
                return response;
            }
            var userDb = await _context.AppUsers.FindAsync(request.UserId);
            if (userDb == null)
            {
                response.Status = false;
                response.Message = "Không tìm thấy tài khoản";
                return response;
            }
            var user = await _userManager.FindByNameAsync(userDb.UserName);
            //  //checkLogin

            if (request.ConfirmPassword != request.NewPassword)
            {
                response.Status = false;
                response.Message = "Mật khẩu mới và nhập xác nhận không trùng khớp";
                return response;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (result.Succeeded)
            {
                response.Status = true;
                response.Message = "Cập nhật mật khẩu thành công";
                return response;

            }
            return response;
        }
        public async Task<ResponseViewModel> ClientChangePassword(ClientUpdatePasswordRequest request)
        {
            var response = new ResponseViewModel();
            if (request.UserId == null)
            {
                response.Status = false;
                response.Message = "Không tìm thấy userId";
                return response;
            }
            var userDb = await _context.AppUsers.FindAsync(request.UserId);
            if (userDb == null)
            {
                response.Status = false;
                response.Message = "Không tìm thấy tài khoản";
                return response;
            }
            var user = await _userManager.FindByNameAsync(userDb.UserName);
            //checkLogin
            var login = await _signInManager.PasswordSignInAsync(user, request.OldPassword, false, true);
            if (!login.Succeeded)
            {
                response.Status = false;
                response.Message = "Bạn nhập sai mật khẩu cũ";
                return response;
            }
            if (request.ConfirmPassword != request.NewPassword)
            {
                response.Status = false;
                response.Message = "Mật khẩu mới và nhập xác nhận không trùng khớp";
                return response;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (result.Succeeded)
            {
                response.Status = true;
                response.Message = "Cập nhật mật khẩu thành công";
                return response;

            }
            return response;
        }

        public async Task<dynamic> GetStatisticalJob(GetUserPagingRequest request)
        {
            var listWorkTags = await _context.TimeShiftUsers.Where(m => m.UserId == request.UserId).ToListAsync();
            var works = _context.TimeShifts.Where(m => m.CreatedId == request.UserId || m.PersonInCharge == request.UserId || listWorkTags.Select(i => i.TimeShiftId).Contains(m.Id));
            if (request.FromDate != null && request.ToDate != null)
            {
                works = works.Where(m => m.StartTime >= request.FromDate && m.EndTime <= request.ToDate);
            }
            var response = new
            {
                Total = await works.CountAsync(),
                TotalDone = await works.Where(m => m.Workflow == Enums.Workflow.COMPLETED).CountAsync(),
                TotalFail = await works.Where(m => m.Workflow == Enums.Workflow.COMPLETED).CountAsync(),
                TotalProcessing = await works.Where(m => m.Workflow == Enums.Workflow.PROCESSING).CountAsync(),
                TotalOverdue = await works.Where(m => m.Workflow == Enums.Workflow.OVERDUE).CountAsync(),
            };

            return response;
        }

        public async Task<dynamic> GetStatisticalByDay(GetUserPagingRequest request)
        {
            var listWorkTags = await _context.TimeShiftUsers.Where(m => m.UserId == request.UserId).ToListAsync();
            var works = _context.TimeShifts.Where(m => m.CreatedId == request.UserId || m.PersonInCharge == request.UserId || listWorkTags.Select(i => i.TimeShiftId).Contains(m.Id))
                .Where(m => m.CreatedTime >= request.FromDate && m.CreatedTime <= request.ToDate);

            var results = works.GroupBy(x => new { Year = x.CreatedTime.Year, Month = x.CreatedTime.Month, Day = x.CreatedTime.Day })
            .Select(x => new
            {
                Value = x.Count(),
                Year = x.Key.Year,
                Month = x.Key.Month,
                Day = x.Key.Day
            }).ToList();
            List<dynamic> selectedDates = new List<dynamic>();
            for (var date = (DateTime)request.FromDate; date <= (DateTime)request.ToDate; date = date.AddDays(1))
            {
                float total = 0;
                var value = new
                {
                    Value = total,
                    Year = date.Year,
                    Month = date.Month,
                    Day = date.Day
                };
                var checkResult = results.FirstOrDefault(r => r.Year == value.Year && r.Month == value.Month && r.Day == value.Day);
                if (checkResult != null)
                {
                    selectedDates.Add(new
                    {
                        Value = checkResult.Value,
                        Year = date.Year,
                        Month = date.Month,
                        Day = date.Day
                    });
                }
                else
                {
                    selectedDates.Add(value);
                }
            }

            return selectedDates;
        }

        public async Task<List<StatisticalTimeShiftTypeViewModel>> GetStatisticalTimeShiftType(GetUserPagingRequest request)
        {

            var query = _context.TimeShiftTypes.Where(m => m.IsDeleted == false && m.TotalTimeShift > 0)
                .OrderByDescending(m => m.TotalTimeShift).AsQueryable();

            if (request.FromDate != null && request.ToDate != null)
            {
                query = query.Where(m => m.StartTime >= request.FromDate && m.EndTime <= request.ToDate);
            }
            //3. paging
            var datapaging = Pagination.getPaging(query, request.PageIndex, request.PageSize);

            var listWorkTags = await _context.TimeShiftUsers.Where(m => m.UserId == request.UserId).ToListAsync();
            var timeshifts = _context.TimeShifts.Where(m => m.CreatedId == request.UserId || listWorkTags.Select(i => i.TimeShiftId).Contains(m.Id))
            .Where(m => query.Select(q => q.Id).Contains(m.TimeShiftTypeId));
            var response = new List<StatisticalTimeShiftTypeViewModel>();
            foreach (var d in datapaging.Data)
            {
                var input = new StatisticalTimeShiftTypeViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    TotalTimeShift = timeshifts.Where(m => m.TimeShiftTypeId == d.Id).Count(),
                    TotalDone = timeshifts.Where(m => m.TimeShiftTypeId == d.Id && m.Workflow == Enums.Workflow.COMPLETED).Count(),
                };
                response.Add(input);
            }


            return response;
        }

        public async Task<PagedResult<TimeShiftDTOViewModel>> GetListTimeShiftTags(GetUserPagingRequest request)
        {
            var listWorkTags = await _context.TimeShiftUsers.Where(m => m.UserId == request.UserId).ToListAsync();
            var query = _context.TimeShifts
                .Where(m => m.IsDeleted == false)

                .Where(m => m.CreatedId == request.UserId || listWorkTags.Select(i => i.TimeShiftId).Contains(m.Id)).AsNoTracking();

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(m => m.Name.Contains(request.Name));
            }
            if (request.FromDate != null && request.ToDate != null)
            {
                query = query.Where(m => m.StartTime >= request.FromDate && m.EndTime <= request.ToDate);
            }
            if (request.TimeShiftTypeId != null)
            {
                query = query.Where(m => m.TimeShiftTypeId == request.TimeShiftTypeId);
            }

            //3. paging
            var datapaging = Pagination.getPaging(query.OrderByDescending(m => m.StartTime), request.PageIndex, request.PageSize);

            //4 .Select and projecttion
            var pagedResult = new PagedResult<TimeShiftDTOViewModel>()
            {
                TotalRecord = datapaging.TotalItems,

            };

            var users = await _context.AppUsers.Where(m => datapaging.Data.Select(i => i.PersonInCharge).Contains(m.Id))
                .Select(m => new UserViewModel
                {
                    Id = m.Id,
                    UserId = m.Id,
                    FullName = m.FullName,
                }).ToListAsync();

            var timeShiftTypes = await _context.TimeShiftTypes.Where(m => datapaging.Data.Select(i => i.TimeShiftTypeId).Contains(m.Id)).ToListAsync();

            foreach (var d in datapaging.Data)
            {

                var input = new TimeShiftDTOViewModel()
                {
                    TimeShift = d,
                };
                var checkUser = users.FirstOrDefault(m => m.Id == d.PersonInCharge);
                if (checkUser != null)
                {
                    input.User = checkUser;
                }
                var checkTimeShifType = timeShiftTypes.FirstOrDefault(m => m.Id == d.TimeShiftTypeId);
                if (checkTimeShifType != null)
                {
                    input.TimeShiftType = checkTimeShifType;
                }
                pagedResult.Items.Add(input);
            }
            await Task.CompletedTask;
            return pagedResult;
        }
    }
}
