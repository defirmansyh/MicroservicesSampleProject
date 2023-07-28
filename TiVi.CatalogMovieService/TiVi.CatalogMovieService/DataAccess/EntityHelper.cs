namespace TiVi.UserCatalogService.DataAccess
{
    public static class EntityHelper
    {
        public static void SetAuditFieldsForInsert(dynamic entity, string username)
        {
            try
            {
                if (username.Equals(null) || username.Equals(""))
                {
                    entity.CreatedBy = "System";
                }
                else 
                {
                    entity.CreatedBy = username;
                }    
                entity.IsActive = true;
                entity.CreatedDate = DateTime.Now;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) { }
        }

        public static void SetAuditFieldsForUpdate(dynamic entity, string username)
        {
            try
            {
                entity.ModifiedDate = DateTime.Now;
                entity.ModifiedBy = username;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) { }
        }

        public static void SetAuditFieldsForDelete(dynamic entity, string username)
        {
            try
            {
                entity.IsActive = false;
                entity.ModifiedDate = DateTime.Now;
                entity.ModifiedBy = username;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) { }
        }
    }
}
