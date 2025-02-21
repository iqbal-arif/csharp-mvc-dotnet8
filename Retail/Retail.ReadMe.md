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
	
VIEWMODEL (Model Specific for View)
 9. Create ViewModel in Retial.Models
 10. @using Retail.Models.ViewModels is where ProductVM is located. Add it to All _ViewImorts files
 11. ValidateNever Attribute for the properties that are not supposed to be validated
 12. ViewModels are specifically designed for strongly typed view. And this ViewModels are called "Strongly Typed View"
 
 UPLOAD A FILE THROUGH PC
 13. Add enctype = "multipart/form-data" in the Form header fields
 14. file type="file" in the html tag

COMBINING CREATE & EDIT PAGES FOR product
15. Use Upsert METHOD in ProductController
	public IActionResult Upsert(ProductVM productVM, IFormFile? file)

RICH TEXT EDITOR
16. Integrate TinyMCE (https://www.tiny.cloud/my-account/integrate/#html)
17. Insert CDN JavaScript tag in html
18. Use TextEditor JavaScript Tag in Product View PAGES

IWEWHOSTENVIRONMENT TO ACCESS LOCAL FILES, a built-in .NET FEATURE

19. Inserting WebHostEnvironment to access Datafolder to access files loacally.

		private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _weHostEnironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment weHostEnironment)
        {
            _unitOfWork = unitOfWork;
            _weHostEnironment = weHostEnironment;
        }
20. Access www Root Folder
21. Get filename with extension
22. Get file path and combine
23. Copy the file
24. Save the FILE

UPDATING THE IMAGE
25. Check for the image FILE
26. Delete the old Image with file path

DATATABLES (SEARCH FUNCTIONALITY, SORTING, PAGINATION)
27. Third-Party Plugin datatables.net (https://datatables.net/)
28. Set Api call to retrive ObjProductList as JSON
		#region API Calls
			[HttpGet]
			public IActionResult GetAll() 
			{
				List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
				return Json(new { data = objProductList });

			}
        #endregion

LOAD DATATABLES from Api
29. Removed all Data table fromatting
@* <tbody> *@
			@* 	@foreach (var obj in Model) *@
			@* 	{ *@
			@* 		<tr> *@
			@* 			<td>@obj.Title</td> *@
			@* 			<td>@obj.ISBN</td> *@
			@* 			<td>@obj.ListPrice</td> *@
			@* 			<td>@obj.Author</td> *@
			@* 			<td>@obj.Category.Name</td> *@
			@* 			<td> *@
			@* 				<div class="w-75 btn-group" role="group"> *@
			@* 					<a asp-controller="Product" asp-action="Upsert" asp-route-id="@obj.Id" class="btn btn-primary mx-2"> *@
			@* 						<i class="bi bi-pencil-square"></i> Edit *@
			@* 					</a> *@
			@* 					<a asp-controller="Product" asp-action="Delete" asp-route-id="@obj.Id" class="btn btn-danger mx-2"> *@
			@* 						<i class="bi bi-journal-x"></i> Delete *@
			@* 					</a> *@
			@* 				</div> *@
			@* 			</td> *@
			@* 		</tr> *@
			@* 	} *@
			@* </tbody> *@

SWEETALERT WHEN DELETING
30. SweetAlter2 https://sweetalert2.github.io/

IDENTITY .NET CORE

31. Install Microsoft.AspNetCore.Identity.EntityFrameworkCore version 8.0.13
32. Add following Configurtion code as, Keys of Idendity tables are Mapped in the OnModelCreating in ApplicationDbContext
        base.OnModelCreating(modelBuilder); 
33. On Project File Add New Scaffolded Item. for Identity. This will make changes to the system files and logic files.
34. Add "<IdentityUser>" to the following function.
		public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	And Delete
	The second instance of ApplicationDbContext added in Identity folder created by Identity Framework
35. Remove Email verificaiton option from Program.cs
	The bottom code is left to to show Email verification Option IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true) has be delete
            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
36. Add User Authentication in Program.cs
            app.UseAuthentication();
37. Removed Another Added Database Connection in appsettings.json 
	"ApplicationDbContextConnection": "Server=(localdb)\\mssqllocaldb;Database=RetailWeb;Trusted_Connection=True;MultipleActiveResultSets=true"			
38. Remove Data folder in Areas/Idendity/ as Data is already available
39. Identity Framework creates Razor Pages related to Idendity.
40. Add _LoginPartial.cshtml into _Layout.cshtml
41. Add Service for Razor Pages to work in Program.cs
            builder.Services.AddRazorPages();
42. Add Razor Pages Mapping in Program.cs
            app.MapRazorPages();	
43. Adding Tables in SQL for Identity .net CORE
	add-migration addIdentityTables
	update-database
44. Create ApplicationUser.cs Class for custom User Entity.
45. Add DbSet for Application Users (Custom User Properties) in ApplicationDbContext.cs
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }	
46. Run Migration
	add-migration ExtendIdentityUser
	update-database