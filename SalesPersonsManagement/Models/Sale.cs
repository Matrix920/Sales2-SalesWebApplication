//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sales_Persons.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sale()
        {
            this.SalesDetails = new HashSet<SalesDetail>();
        }
    
        public int SaleID { get; set; }
        public System.DateTime Date { get; set; }
        public int PersonID { get; set; }
        public int ComissionID { get; set; }
    
        public virtual SalesPerson SalesPerson { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesDetail> SalesDetails { get; set; }
        public virtual Comission Comission { get; set; }
    }
}