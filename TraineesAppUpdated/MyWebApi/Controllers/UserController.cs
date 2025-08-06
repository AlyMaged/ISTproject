using Trainees.Models.Interfaces.Base;
using Trainees.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CommonLib.Responses;
using CommonLib.ResultCodes;
using CommonLib.EnumExtensionMethods;
using MapperHelper;
using Trainees.Models.ModelsDTO;
using System.Web.Http.Cors;

namespace MyWebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : HiveAPIBaseController
    {
        public UserController(IUnitOfWork _dataUnitOfWork)
           : base(_dataUnitOfWork)
        {
        }

        [HttpGet]
        [Route("API/User/GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                List<CFMUser> users = dataUnitOfWork.CFMUsers.GetAll().ToList();
                List<CFMUserDTO> usersDTO = users.MapTo<List<CFMUserDTO>>();

                if (usersDTO.Count > 0)
                {
                    return InterfaceJson(new ResponsePackage<List<CFMUserDTO>> { Result = usersDTO });
                }
                else
                {
                    return InterfaceJson(new ResponsePackage<object>(
                    new Error
                    {
                        Code = ResultCode.NoItemsFound,
                        Message = ResultCode.NoItemsFound.GetDescription()
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return InterfaceJson(new ResponsePackage<object>(
                new Error()
                {
                    Code = ResultCode.APIPostFailed,
                    Message = ResultCode.APIPostFailed.GetDescription()
                }));
            }
        }

        [HttpGet]
        [Route("API/User/GetById")]
        public IHttpActionResult GetById([FromUri] int Id)
        {
            try
            {
                CFMUser user = dataUnitOfWork.CFMUsers.Get(Id);
                CFMUserDTO userDTO = user.MapTo<CFMUserDTO>();

                if (userDTO != null)
                {
                    return InterfaceJson(new ResponsePackage<CFMUserDTO> { Result = userDTO });
                }
                else
                {
                    return InterfaceJson(new ResponsePackage<object>(
                    new Error
                    {
                        Code = ResultCode.NoItemsFound,
                        Message = ResultCode.NoItemsFound.GetDescription()
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return InterfaceJson(new ResponsePackage<object>(
                new Error()
                {
                    Code = ResultCode.APIPostFailed,
                    Message = ResultCode.APIPostFailed.GetDescription()
                }));
            }
        }

        // -------------------- [CREATE USER] --------------------
        [HttpPost]
        [Route("API/User/Create")]
        public IHttpActionResult Create([FromBody] CFMUserDTO userDTO)
        {
            try
            {
                CFMUser user = userDTO.MapTo<CFMUser>();
                dataUnitOfWork.CFMUsers.Add(user);
                dataUnitOfWork.Complete();

                return InterfaceJson(new ResponsePackage<string> { Result = "User created successfully." });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return InterfaceJson(new ResponsePackage<object>(new Error()
                {
                    Code = ResultCode.APIPostFailed,
                    Message = "User creation failed."
                }));
            }
        }
        // -------------------- [END CREATE USER] --------------------

        // -------------------- [UPDATE USER (PATCH)] --------------------
        // PATCH: API/User/Update
        [HttpPatch]
        [Route("API/User/Update")]
        public IHttpActionResult Update([FromBody] CFMUserDTO model)
        {
            try
            {
                var userInDb = dataUnitOfWork.CFMUsers.Get(model.ID);

                if (userInDb == null)
                {
                    return InterfaceJson(new ResponsePackage<object>(new Error
                    {
                        Code = ResultCode.NoItemsFound,
                        Message = "User not found"
                    }));
                }
                var updated = new CFMUser()
                {
                    ID = model.ID,
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                };
          
                // ✅ Correct usage of Update and Complete
                dataUnitOfWork.CFMUsers.Update(userInDb, updated);
                dataUnitOfWork.Complete();

                return InterfaceJson(new ResponsePackage<string> { Result = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return InterfaceJson(new ResponsePackage<object>(new Error
                {
                    Code = ResultCode.APIPostFailed,
                    Message = ex.Message
                }));
            }
        }

        // -------------------- [END UPDATE USER (PATCH)] --------------------

        // -------------------- [DELETE USER] --------------------
        [HttpDelete]
        [Route("API/User/Delete")]
        public IHttpActionResult Delete([FromUri] int id)
        {
            try
            {
                var user = dataUnitOfWork.CFMUsers.Get(id);
                if (user == null)
                {
                    return InterfaceJson(new ResponsePackage<object>(new Error
                    {
                        Code = ResultCode.NoItemsFound,
                        Message = "User not found."
                    }));
                }

                dataUnitOfWork.CFMUsers.Delete(u => u.ID == id);
                dataUnitOfWork.Complete();

                return InterfaceJson(new ResponsePackage<string> { Result = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return InterfaceJson(new ResponsePackage<object>(new Error
                {
                    Code = ResultCode.APIPostFailed,
                    Message = "User deletion failed."
                }));
            }
        }
        // -------------------- [END DELETE USER] --------------------

        // -------------------- [USER LOGIN] --------------------
        [HttpPost]
        [Route("API/User/Login")]
        public IHttpActionResult Login([FromBody] UserLoginDTO credentials)
        {
            try
            {
                var user = dataUnitOfWork.CFMUsers
                    .Find(u => u.Email == credentials.Email && u.Password == credentials.Password)
                    .FirstOrDefault();

                if (user != null)
                {
                    return InterfaceJson(new ResponsePackage<CFMUserDTO> { Result = user.MapTo<CFMUserDTO>() });
                }
                else
                {
                    return InterfaceJson(new ResponsePackage<object>(new Error
                    {
                        Code = ResultCode.InvalidCredentials,
                        Message = "Invalid email or password."
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return InterfaceJson(new ResponsePackage<object>(new Error
                {
                    Code = ResultCode.APIPostFailed,
                    Message = "Login failed."
                }));
            }
        }
        // -------------------- [END USER LOGIN] --------------------
    }
}
