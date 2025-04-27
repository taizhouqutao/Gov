using Common;
using DAL;
using DAL.Contexts;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
namespace DAL
{
    public class DalRight
    {
        public async Task<List<RightTreeDto>> GetRightTreeAsync(RightReqDto req) {
            var res=new List<Right>();
            using (var context = new webapplicationContext())
            {
                var Query= context.Rights.AsQueryable();
                var QureyRes = Query.Where(i=>
                    (i.IfDel==0)
                );
                res = await QureyRes.ToListAsync();
            }
            return TransFormTree(res);
        }

        private static List<RightTreeDto> TransFormTree(List<Right> Rights)
        {
            var result = new List<RightTreeDto>();
            // 获取所有根节点
            var roots = Rights.Where(r => r.FatherId == 0).ToList();
            // 递归构建树结构
            foreach (var root in roots)
            {
                var treeNode = new RightTreeDto
                {
                    id  = root.Id,
                    rightName = root.RightName,
                    fatherId = root.FatherId,
                    rightCode=root.RightCode,
                    rightType=root.RightType,
                    CreateTime=root.CreateTime,
                    CreateUserId=root.CreateUserId,
                    children = GetChildren(Rights, root.Id)
                };
                result.Add(treeNode);
            }

            return result;
        }

        private static List<RightTreeDto> GetChildren(List<Right> rights, int parentId)
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
                    rightCode=child.RightCode,
                    rightType=child.RightType,
                    CreateTime=child.CreateTime,
                    CreateUserId=child.CreateUserId,
                    children = GetChildren(rights, child.Id)
                };
                children.Add(childTreeNode);
            }

            return children;
        }
    }
}