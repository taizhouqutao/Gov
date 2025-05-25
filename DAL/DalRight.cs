using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DalRight
    {
        public async Task SaveRoleRights(RightReqDto req)
        {
            using (var context = new webapplicationContext())
            {
                var RightsQuery = context.RightRoles.AsQueryable();
                var DelQueryRightRes = RightsQuery.Where(i =>
                    (i.RoleId == req.roleId) &&
                    ((req == null || req.rightIds == null) ? true : !req.rightIds.Contains(i.RightId))
                );
                var DelRoleRights = await DelQueryRightRes.ToListAsync();
                if (DelRoleRights.Count > 0)
                {
                    context.RightRoles.RemoveRange(DelRoleRights);
                }
                var HasQueryRightRes = await RightsQuery.Where(i =>
                    (i.RoleId == req.roleId) &&
                    ((req == null || req.rightIds == null) ? true : req.rightIds.Contains(i.RightId))
                ).ToListAsync();
                var AddRights = (req?.rightIds ?? new List<int>()).Where(j => !HasQueryRightRes.ConvertAll(j => j.RightId).Contains(j)).ToList();
                if (AddRights.Count > 0)
                {
                    await context.RightRoles.AddRangeAsync(AddRights.ConvertAll(i => new RightRole()
                    {
                        CreateTime = DateTime.Now,
                        CreateUserId = req?.UserId ?? 0,
                        RightId = i,
                        IfDel = 0,
                        RoleId = req?.roleId ?? 0,
                    }));
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<RightTreeDto>> GetRightTreeAsync(RightReqDto req)
        {
            var res = new List<Right>();
            var RoleRights = new List<RightRole>();
            using (var context = new webapplicationContext())
            {
                var Query = context.Rights.AsQueryable();
                var QureyRes = Query.Where(i =>
                    (i.IfDel == 0)
                );
                res = await QureyRes.ToListAsync();
                var RightsQuery = context.RightRoles.AsQueryable();
                var QueryRightRes = RightsQuery.Where(i =>
                    (i.IfDel == 0) &&
                    (i.RoleId == req.roleId)
                );
                RoleRights = await QueryRightRes.ToListAsync();
            }

            return TransFormTree(res, RoleRights);
        }

        private static List<RightTreeDto> TransFormTree(List<Right> Rights, List<RightRole> RoleRights)
        {
            var result = new List<RightTreeDto>();
            // 获取所有根节点
            var roots = Rights.Where(r => r.FatherId == 0).ToList();
            // 递归构建树结构
            foreach (var root in roots)
            {
                var treeNode = new RightTreeDto
                {
                    id = root.Id,
                    rightName = root.RightName,
                    fatherId = root.FatherId,
                    rightCode = root.RightCode,
                    rightType = root.RightType,
                    CreateTime = root.CreateTime,
                    CreateUserId = root.CreateUserId,
                    ifchecked = RoleRights.Exists(i => i.RightId == root.Id) ? 1 : 0,
                    children = GetChildren(Rights, root.Id, RoleRights)
                };
                result.Add(treeNode);
            }

            return result;
        }

        private static List<RightTreeDto> GetChildren(List<Right> rights, int parentId, List<RightRole> RoleRights)
        {
            var children = new List<RightTreeDto>();
            var childNodes = rights.Where(r => r.FatherId == parentId).ToList();

            foreach (var child in childNodes)
            {
                var childTreeNode = new RightTreeDto
                {
                    id = child.Id,
                    rightName = child.RightName,
                    fatherId = child.FatherId,
                    rightCode = child.RightCode,
                    rightType = child.RightType,
                    CreateTime = child.CreateTime,
                    CreateUserId = child.CreateUserId,
                    children = GetChildren(rights, child.Id, RoleRights),
                    ifchecked = RoleRights.Exists(i => i.RightId == child.Id) ? 1 : 0,
                };
                children.Add(childTreeNode);
            }

            return children;
        }
    }
}