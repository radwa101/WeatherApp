//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoDatabase
{
    using System;
    using System.Collections.Generic;
    
    public partial class fd_file_definition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fd_file_definition()
        {
            this.ft_file_transaction = new HashSet<ft_file_transaction>();
        }
    
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string HeaderTemplate { get; set; }
        public string TrailerTemplate { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUserId { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ft_file_transaction> ft_file_transaction { get; set; }
    }
}
