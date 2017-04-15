﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tabemashou_User.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TabemashouEntities : DbContext
    {
        public TabemashouEntities()
            : base("name=TabemashouEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Achievement> Achievement { get; set; }
        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Canton> Canton { get; set; }
        public virtual DbSet<Check> Check { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Dish> Dish { get; set; }
        public virtual DbSet<DishesByCheck> DishesByCheck { get; set; }
        public virtual DbSet<DishesPerLocal> DishesPerLocal { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Local> Local { get; set; }
        public virtual DbSet<PARAMETER> PARAMETER { get; set; }
        public virtual DbSet<PaymentByCustomer> PaymentByCustomer { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<Table> Table { get; set; }
        public virtual DbSet<Type> Type { get; set; }
        public virtual DbSet<User> User { get; set; }
    
        public virtual int PR_CreateAdministrator(Nullable<decimal> idCard, string username, string password, string gender, Nullable<System.DateTime> birthDate, Nullable<int> nationality, string firstName, string middleName, string lastName, string secondLastName)
        {
            var idCardParameter = idCard.HasValue ?
                new ObjectParameter("IdCard", idCard) :
                new ObjectParameter("IdCard", typeof(decimal));
    
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var genderParameter = gender != null ?
                new ObjectParameter("Gender", gender) :
                new ObjectParameter("Gender", typeof(string));
    
            var birthDateParameter = birthDate.HasValue ?
                new ObjectParameter("BirthDate", birthDate) :
                new ObjectParameter("BirthDate", typeof(System.DateTime));
    
            var nationalityParameter = nationality.HasValue ?
                new ObjectParameter("Nationality", nationality) :
                new ObjectParameter("Nationality", typeof(int));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var middleNameParameter = middleName != null ?
                new ObjectParameter("MiddleName", middleName) :
                new ObjectParameter("MiddleName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var secondLastNameParameter = secondLastName != null ?
                new ObjectParameter("SecondLastName", secondLastName) :
                new ObjectParameter("SecondLastName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_CreateAdministrator", idCardParameter, usernameParameter, passwordParameter, genderParameter, birthDateParameter, nationalityParameter, firstNameParameter, middleNameParameter, lastNameParameter, secondLastNameParameter);
        }
    
        public virtual int PR_CreateCustomer(Nullable<decimal> idCard, string username, string password, string gender, Nullable<System.DateTime> birthDate, Nullable<int> nationality, string firstName, string middleName, string lastName, string secondLastName, byte[] photo, Nullable<decimal> accountNumber)
        {
            var idCardParameter = idCard.HasValue ?
                new ObjectParameter("IdCard", idCard) :
                new ObjectParameter("IdCard", typeof(decimal));
    
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var genderParameter = gender != null ?
                new ObjectParameter("Gender", gender) :
                new ObjectParameter("Gender", typeof(string));
    
            var birthDateParameter = birthDate.HasValue ?
                new ObjectParameter("BirthDate", birthDate) :
                new ObjectParameter("BirthDate", typeof(System.DateTime));
    
            var nationalityParameter = nationality.HasValue ?
                new ObjectParameter("Nationality", nationality) :
                new ObjectParameter("Nationality", typeof(int));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var middleNameParameter = middleName != null ?
                new ObjectParameter("MiddleName", middleName) :
                new ObjectParameter("MiddleName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var secondLastNameParameter = secondLastName != null ?
                new ObjectParameter("SecondLastName", secondLastName) :
                new ObjectParameter("SecondLastName", typeof(string));
    
            var photoParameter = photo != null ?
                new ObjectParameter("Photo", photo) :
                new ObjectParameter("Photo", typeof(byte[]));
    
            var accountNumberParameter = accountNumber.HasValue ?
                new ObjectParameter("AccountNumber", accountNumber) :
                new ObjectParameter("AccountNumber", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_CreateCustomer", idCardParameter, usernameParameter, passwordParameter, genderParameter, birthDateParameter, nationalityParameter, firstNameParameter, middleNameParameter, lastNameParameter, secondLastNameParameter, photoParameter, accountNumberParameter);
        }
    
        public virtual int PR_CreateDetailCheck(Nullable<int> idDish, Nullable<int> idCheck)
        {
            var idDishParameter = idDish.HasValue ?
                new ObjectParameter("IdDish", idDish) :
                new ObjectParameter("IdDish", typeof(int));
    
            var idCheckParameter = idCheck.HasValue ?
                new ObjectParameter("IdCheck", idCheck) :
                new ObjectParameter("IdCheck", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_CreateDetailCheck", idDishParameter, idCheckParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> PR_CreateLocal(Nullable<int> idRestaurant, Nullable<int> idDistrict, string detail, string latitude, string longitude)
        {
            var idRestaurantParameter = idRestaurant.HasValue ?
                new ObjectParameter("IdRestaurant", idRestaurant) :
                new ObjectParameter("IdRestaurant", typeof(int));
    
            var idDistrictParameter = idDistrict.HasValue ?
                new ObjectParameter("IdDistrict", idDistrict) :
                new ObjectParameter("IdDistrict", typeof(int));
    
            var detailParameter = detail != null ?
                new ObjectParameter("Detail", detail) :
                new ObjectParameter("Detail", typeof(string));
    
            var latitudeParameter = latitude != null ?
                new ObjectParameter("Latitude", latitude) :
                new ObjectParameter("Latitude", typeof(string));
    
            var longitudeParameter = longitude != null ?
                new ObjectParameter("Longitude", longitude) :
                new ObjectParameter("Longitude", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("PR_CreateLocal", idRestaurantParameter, idDistrictParameter, detailParameter, latitudeParameter, longitudeParameter);
        }
    
        public virtual int PR_CreatePaymentCheck(Nullable<decimal> idCard, Nullable<int> idCheck)
        {
            var idCardParameter = idCard.HasValue ?
                new ObjectParameter("IdCard", idCard) :
                new ObjectParameter("IdCard", typeof(decimal));
    
            var idCheckParameter = idCheck.HasValue ?
                new ObjectParameter("IdCheck", idCheck) :
                new ObjectParameter("IdCheck", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_CreatePaymentCheck", idCardParameter, idCheckParameter);
        }
    
        public virtual int PR_CreateUser(Nullable<decimal> idCard, string username, string password, string gender, Nullable<System.DateTime> birthDate, Nullable<int> nationality, string firstName, string middleName, string lastName, string secondLastName)
        {
            var idCardParameter = idCard.HasValue ?
                new ObjectParameter("IdCard", idCard) :
                new ObjectParameter("IdCard", typeof(decimal));
    
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var genderParameter = gender != null ?
                new ObjectParameter("Gender", gender) :
                new ObjectParameter("Gender", typeof(string));
    
            var birthDateParameter = birthDate.HasValue ?
                new ObjectParameter("BirthDate", birthDate) :
                new ObjectParameter("BirthDate", typeof(System.DateTime));
    
            var nationalityParameter = nationality.HasValue ?
                new ObjectParameter("Nationality", nationality) :
                new ObjectParameter("Nationality", typeof(int));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var middleNameParameter = middleName != null ?
                new ObjectParameter("MiddleName", middleName) :
                new ObjectParameter("MiddleName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var secondLastNameParameter = secondLastName != null ?
                new ObjectParameter("SecondLastName", secondLastName) :
                new ObjectParameter("SecondLastName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_CreateUser", idCardParameter, usernameParameter, passwordParameter, genderParameter, birthDateParameter, nationalityParameter, firstNameParameter, middleNameParameter, lastNameParameter, secondLastNameParameter);
        }
    
        public virtual int PR_DeleteDishPhoto(Nullable<int> idDish, Nullable<int> idPhoto)
        {
            var idDishParameter = idDish.HasValue ?
                new ObjectParameter("IdDish", idDish) :
                new ObjectParameter("IdDish", typeof(int));
    
            var idPhotoParameter = idPhoto.HasValue ?
                new ObjectParameter("IdPhoto", idPhoto) :
                new ObjectParameter("IdPhoto", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_DeleteDishPhoto", idDishParameter, idPhotoParameter);
        }
    
        public virtual int PR_DeleteLocal(Nullable<int> idLocal)
        {
            var idLocalParameter = idLocal.HasValue ?
                new ObjectParameter("IdLocal", idLocal) :
                new ObjectParameter("IdLocal", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_DeleteLocal", idLocalParameter);
        }
    
        public virtual int PR_DeleteLocalPhoto(Nullable<int> idLocal, Nullable<int> idPhoto)
        {
            var idLocalParameter = idLocal.HasValue ?
                new ObjectParameter("IdLocal", idLocal) :
                new ObjectParameter("IdLocal", typeof(int));
    
            var idPhotoParameter = idPhoto.HasValue ?
                new ObjectParameter("IdPhoto", idPhoto) :
                new ObjectParameter("IdPhoto", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_DeleteLocalPhoto", idLocalParameter, idPhotoParameter);
        }
    
        public virtual int PR_DeleteRestaurant(Nullable<int> restaurantId)
        {
            var restaurantIdParameter = restaurantId.HasValue ?
                new ObjectParameter("RestaurantId", restaurantId) :
                new ObjectParameter("RestaurantId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_DeleteRestaurant", restaurantIdParameter);
        }
    
        public virtual int PR_DeleteRestaurantTypes(Nullable<int> restaurantId)
        {
            var restaurantIdParameter = restaurantId.HasValue ?
                new ObjectParameter("RestaurantId", restaurantId) :
                new ObjectParameter("RestaurantId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_DeleteRestaurantTypes", restaurantIdParameter);
        }
    
        public virtual ObjectResult<PR_GetDistricts_Result> PR_GetDistricts()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PR_GetDistricts_Result>("PR_GetDistricts");
        }
    
        public virtual ObjectResult<Nullable<decimal>> PR_NewCheck(Nullable<int> idLocal)
        {
            var idLocalParameter = idLocal.HasValue ?
                new ObjectParameter("IdLocal", idLocal) :
                new ObjectParameter("IdLocal", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("PR_NewCheck", idLocalParameter);
        }
    
        public virtual ObjectResult<PR_RestaurantInfo_Result> PR_RestaurantInfo(Nullable<decimal> aminId)
        {
            var aminIdParameter = aminId.HasValue ?
                new ObjectParameter("AminId", aminId) :
                new ObjectParameter("AminId", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PR_RestaurantInfo_Result>("PR_RestaurantInfo", aminIdParameter);
        }
    
        public virtual int PR_UpdateRestaurantTypes(Nullable<int> restaurantId, string typeList)
        {
            var restaurantIdParameter = restaurantId.HasValue ?
                new ObjectParameter("RestaurantId", restaurantId) :
                new ObjectParameter("RestaurantId", typeof(int));
    
            var typeListParameter = typeList != null ?
                new ObjectParameter("TypeList", typeList) :
                new ObjectParameter("TypeList", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_UpdateRestaurantTypes", restaurantIdParameter, typeListParameter);
        }
    }
}
