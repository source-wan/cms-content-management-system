using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Domain.Base
{
   public abstract class BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <value></value>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// （最后）更新时间
        /// </summary>
        /// <value></value>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 创建人（的Id）
        /// </summary>
        /// <value></value>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// （最后）更新人（的Id）
        /// </summary>
        /// <value></value>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// 是否启用，默认是
        /// </summary>
        /// <value></value>
        public bool IsActived { get; set; }

        /// <summary>
        /// 是否删除，默认否
        /// </summary>
        /// <value></value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        public string? Remarks { get; set; }
    }
}