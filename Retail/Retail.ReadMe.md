RETAIL STORE WEB APP

1. Create the following Class LIBRARY
	CLASS LIBRARY - BULKY.DataAccess (Contains all the Db related code)
	CLASS LIBRARY - BULKY.Models (Contains all the Models for the project)
	CLASS LIBRARY - BULKY.Utility (Contains email functionality related code)
	
2. Install a new BootStrap Theme

3. Create SD Class to store website constants in it.
4. Install Nuget Package EntityFrameworkCore & EntityFrameworkCore.Tools for Retail.DataAccess
5. Delete the old Db and Migration
6. add-migration AddCategoryToDbAndSeedTable => Make sure you change the Default project to where the Data is sought from. In this case it is Retail.DataAccess
7. update-database

4. IREPOSITROY INTERFACE: (where generic fields and methods are defined)
5. Define genericIRepository Interface Class
6. Define fields and methods (to perform CRUD operation or interact with DB)
7. T  - CATEGORY
	1. Get All Category  :IEnumerable<T> GetAll();
	2. Get a single first Category : T Get(Expression<Func<T, bool>> filter); // Expression is a LINQ Operator
	3. Add, Remove, RemoveRange Repository
8. IMPLEMENTING IREPOSITROY;
9. CATEGORY TO IMPLEMENT IREPOSITROY
10.IMPLEMENTING CATEGORY Repository with base functionality and update and save features.
11.NOTE: Remember to Register Service to Dependency Injection Life : Scoped, Singleton ,etc


PRODUCCT Model
1. Add Properties for product in Products.cs Model
2. Enter Data Manually in ApplicationDbContext for testing purpose.
3. Create Product Table in Database: 
	add-migration addProductsToDb
	update-database

PRODUCT CONTROLLER & ACTION METHOD
// Ctrl+Shift+F (Find & Replace)
// Alt+Shift+; Select every instance of same word
//AltShift+. Select next same word

CATEGORY and PRODUCCT RELATION
1. public int CategoryId { get; set; }
   [ForeignKey("CategoryId")]
   public Category Category { get; set; }
2. add-migration addForeignKeyForCategoryProductRelation  // to create relationship between 2 tables
3. remove-migration // To remove last migration.
4. add-migration AddCategoryToDbAndSeedTable
5. add-migration addProductsToDb
6. add-migration addForeignKeyForCategoryProductRelation
7. update-database
8. add-migration addImageUrlToProduct

VIEWBAG ASP-ITEM (Expects IEnumerable of SelectListItem)
	<div class="form-floating py-2 col-12">
		<select asp-for="CategoryId" asp-items = "ViewBag.CategoryList" class="form-control boarder-0 shadow" >
			<option disabled selected>--Select Cagtegory--</option>
		</select>
		<label asp-for="CategoryId" class="ms-2"></label>
		<span asp-validation-for="CategoryId" class="text-danger"></span>
	</div>
