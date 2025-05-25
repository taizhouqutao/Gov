using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
namespace DAL
{
    public class DalUser
    {
        public async Task<PageList<User>> GetUsersByPageAsync(PageReq<UserReqDto> req)
        {
            var res = new List<User>();
            int total = 0, allcount = 0;
            using (var context = new webapplicationContext())
            {
                var Query = context.Users.AsQueryable();
                var QureyRes = Query.Where(i =>
                    (i.IfDel == 0) &&
                    ((req.search == null || string.IsNullOrEmpty(req.search.value)) ? true : i.UserName.Contains(req.search.value))
                );
                QureyRes = QureyRes.OrderByDescending(j => j.Id);
                res = await QureyRes.Skip(req.start).Take(req.length).ToListAsync();
                total = await QureyRes.CountAsync();
                allcount = await context.Users.CountAsync(i => i.IfDel == 0);
            }
            return new PageList<User>()
            {
                data = res,
                recordsFiltered = total,
                draw = req.draw,
                recordsTotal = allcount
            };
        }

        public async Task<List<string>> GetRightCodeByUser(int UserId)
        {
            List<string> res = new List<string>();
            using (var context = new webapplicationContext())
            {
                var Query = context.UserRoles.AsQueryable();
                var QueryRightRoles = context.RightRoles.AsQueryable();
                var QueryRights = context.Rights.AsQueryable();
                var userRoles = (await Query.Where(i =>
                    (i.IfDel == 0) &&
                    (i.UserId == UserId)
                ).ToListAsync()).ConvertAll(i => i.RoleId);
                var RightIds = (await QueryRightRoles.Where(i =>
                                   (i.IfDel == 0) &&
                                   (userRoles.Contains(i.RoleId))
                               ).Select(i => i.RightId).Distinct().ToListAsync()).ConvertAll(i => i);
                res = await QueryRights.Where(i =>
                                (i.IfDel == 0) &&
                                (RightIds.Contains(i.Id))
                ).Select(i => i.RightCode).Distinct().ToListAsync();
            }
            return res;
        }

        public async Task<List<User>> GetUsersByIdAsync(List<int> Ids)
        {
            List<User> res = new List<User>();
            using (var context = new webapplicationContext())
            {
                var Query = context.Users.AsQueryable();
                res = await Query.Where(i =>
                    (i.IfDel == 0) &&
                    (Ids.Contains(i.Id))
                ).ToListAsync();
            }
            return res;
        }

        public async Task<List<User>> GetUsersAsync(UserReqDto req)
        {
            var res = new List<User>();
            using (var context = new webapplicationContext())
            {
                var Query = context.Users.AsQueryable();
                var QureyRes = Query.Where(i =>
                    (i.IfDel == 0) &&
                    ((req.userName == null || string.IsNullOrEmpty(req.userName)) ? true : i.UserName == req.userName) &&
                    ((req.id == null || req.id == 0) ? true : i.Id == req.id) &&
                    ((req.idNot == null || req.idNot == 0) ? true : i.Id != req.idNot)
                );
                res = await QureyRes.ToListAsync();
            }
            return res;
        }

        public async Task<User?> LoginAsync(LoginReqDto req)
        {
            User? res = null;
            using (var context = new webapplicationContext())
            {
                var Query = context.Users.AsQueryable();
                res = await Query.FirstOrDefaultAsync(i =>
                    (i.IfDel == 0) &&
                    (req.UserName.Trim() == i.UserName.Trim()) &&
                    (req.PassWord.Trim() == i.PassWord.Trim())
                );
            }
            return res;
        }

        public async Task<User?> GetUserByIdAsync(int Id)
        {
            User? res = null;
            using (var context = new webapplicationContext())
            {
                var Query = context.Users.AsQueryable();
                res = await Query.FirstOrDefaultAsync(i =>
                    (i.IfDel == 0) &&
                    (i.Id == Id)
                );
            }
            return res;
        }

        public async Task<User?> AddUserAsync(User entity)
        {
            User? res = null;
            using (var context = new webapplicationContext())
            {
                res = (await context.Users.AddAsync(entity)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }

        public async Task<User?> UpdateUserAsync(User entity)
        {
            User? res = null;
            using (var context = new webapplicationContext())
            {
                res = (context.Users.Update(entity)).Entity;
                await context.SaveChangesAsync();
            }
            return res;
        }

        public async Task UpdateUsersAsync(List<User> entitys)
        {
            using (var context = new webapplicationContext())
            {
                context.Users.UpdateRange(entitys);
                await context.SaveChangesAsync();
            }
        }

        public async Task DelUserAsync(List<int> Ids)
        {
            using (var context = new webapplicationContext())
            {
                var Users = await context.Users.Where(i => Ids.Contains(i.Id)).ToListAsync();
                Users.ForEach(i =>
                {
                    i.IfDel = 1;
                });
                context.Users.UpdateRange(Users);
                await context.SaveChangesAsync();
            }
        }

        public async Task SaveUserRole(User entity, List<int> RoleIds)
        {
            using (var context = new webapplicationContext())
            {
                var DeluserRoles = await context.UserRoles.Where(i =>
                    (i.IfDel == 0) &&
                    (i.UserId == entity.Id) &&
                    (!RoleIds.Contains(i.RoleId))
                ).ToListAsync();
                if (DeluserRoles.Count > 0)
                {
                    context.UserRoles.RemoveRange(DeluserRoles);
                }
                var HasuserRoles = await context.UserRoles.Where(i =>
                   (i.IfDel == 0) &&
                   (i.UserId == entity.Id) &&
                   (RoleIds.Contains(i.RoleId))
               ).ToListAsync();
                var AddRoleIds = RoleIds.Where(j => !HasuserRoles.ConvertAll(k => k.RoleId).Contains(j)).ToList().ConvertAll(i => new UserRole()
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = entity.UpdateUserId ?? entity.CreateUserId,
                    IfDel = 0,
                    RoleId = i,
                    UserId = entity.Id,
                });
                if (AddRoleIds.Count > 0)
                {
                    await context.UserRoles.AddRangeAsync(AddRoleIds);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}