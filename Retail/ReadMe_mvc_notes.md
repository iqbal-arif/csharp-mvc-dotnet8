***Program.cs***
***
1. Builder Services
	var builder = WebApplication.CreatBuilder(args);
	NOTE:
		1. By Default MVC services are added
		2. All new services will be added including Dependency Injections in this area
		3. Primary Key Annotation (System.ComponentModel.DataAnnotations.KeyAttribute
		4. SQL Server Management
			1. Login through Client Machine Name with Windows Authentication
			2. Login through . Machine Name with Windows Authentication
			3. Login through localhost Machine Name with Windows Authentication
			4. Login through (LocalDb)\MSSQLLocalDB Machine Name with Windows Authentication
		5.Defining ConnectionString in appsettings.json file as follows;
			"ConnectionStrings": {"Server": ".;Database=Retail;Trusted_Connection=True;TrustServerCertificate=True"}
		6.Nuget Package and source installation
			1. Nuget Source URL: https://api.nuget.org/v3/index.json
			2. Install the following
				a. MICROSOFT.ENTITYFRAMEWORKCORE.TOOLS for Entity Framework Core Tools for the NuGet Package Manager Console in Visual Studio
				b. MICROSOFT.ENTITYFRAMEWORKCORE is a modern object-database mapper for .NET
				d. MICROSOFT.ENTITYFRAMEWORKCORE.SQLSERVER Microsoft SQL Server database provider for Entity Framework Core.
			3. Confirm the Package files in Edit Project file for the Project
		7. Configuration for DataBase Connection(Basic syntax or Basic Classes Configuration in order to use EntityFrameworkCore)
			1. DbContext (Root Class) : 
				public class ApplicationDbContext : DbContext
				{
					// Passing connection string from appsettings.json to DbContext
					public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
					{
						
					}
				}
		8. Adding Services in Program.cs to be able to use the EntityFrameworkCore in Application for Database creation
			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
			
			(Adding EntityFrameworkCore service and configuring the db connection string from appstettings to Program.cs
		9. Setting a New Database
			Nuget Console : update-Database (To create a Db)
		10. Creating "Categories" Table in SQL Db
			a. ApplicationDbContext.cs
				// Creates DataBase Table name as "Categories"
				public DbSet<Category> Categories { get; set; }
				CL: 
				First Command: add-migration AddCateoryTableToDb (To create Tables with primary ID)
				Second Command: update-Database
		CREATING DB FOR CategoryTable 	
		11. Steps to Create db Table
			A. Create a MODEL & Set Table Properties
			B. In DbContext, create Db Set
			C. In Package Manager Console: update-Database
			C. In Package Manager Console: add a migration 
			   (run command: add-migration AddCateoryTableToDb)
			B. In Package Manager Console: 			
			   (run Command: update-Database)
		CREATING CategoryController, Setting Link in _Layout
		12. Create Controller: CategoryController.cs
		13. Create Views: /Category/indexcshtml
		14. Create navigation link in _Layout.cshtml
		
		CREATING Categories List in Db
		15. Create Category in Db through ModelBuilder Helper Function in ApplicationDbContext
			protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
				modelBuilder.Entity<Category>().HasData(
					new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
					new Category { Id = 2, Name = "Scifi", DisplayOrder = 2 },
					new Category { Id = 3, Name = "History", DisplayOrder = 3 }
					);
			}
		16. Always Add migration command to make changes in Db :
			(run Command: "add-migration SeedCategoryTab")
			(run Command: update-Database)
			
		RETRIVING CATEGORY LIST IN CategoryController
		17. Retrive the ApplicationDbContext implementation in CategoryController
			// Field to retrieve db object
			private readonly ApplicationDbContext _db;
			// Constructor
			public CategoryController(ApplicationDbContext db)
			{
				_db = db;
			}
			public IActionResult Index()
			{
				//Retriving all the Categories list
				List<Category> objCategoryList = _db.Categories.ToList();
				return View();
			}
		
		ADDING TABLE VIEW IN Category index.cshtml file
		18. 
		<table class="table table-bordered table-striped">
		<tr>
			<th>Category Name</th>
			<th>Display Order</th>
		</tr>
		</table>
		
		IMPORTING CATEGORY LIST in the Table View in Category index.cshtml file
		19.
		@model List<Category>

		<h1>Category List</h1>
		<table class="table table-bordered table-striped">
			<thead>
			<tr>
				<th>Category Name</th>
				<th>Display Order</th>
			</tr>
			</thead>
			<tbody>
				@foreach(var obj in Model.OrderBy(list=>list.DisplayOrder))
					{
						<tr>
							<td>@obj.Name</td>
							<td>@obj.DisplayOrder</td>
						</tr>
					}
			</tbody>
		</table>
		
		ACTION METHOD TO CREATE CATEGORY UI
		20. Add Action Method in CategoryController.cs
		//Action Method for Create Categry
		public IActionResult Create()
		{
			return View();
		}
		21. Adding Controller and Action in Category Index.cshtml
		<div class="col-6 text-end">
		<a asp-controller="Category" asp-action="Create" class="btn btn-primary">
		<i class="bi bi-database-add p-2"></i>Create New Category
		</a>
		</div>
		
		TAG HELPER
		22. Adding Tags in Category MODEL
			public class Category
			{
			[Key]   
				public int Id { get; set; }
				[Required]
				[DisplayName("Category Name")]
				public string Name { get; set; }

				[DisplayName("Display Order")]
				public int DisplayOrder { get; set; }
			}
		23. Adding Tags in Create.cshtml View
			<div class="mb-3 row p-1">
			<label asp-for="Name" class="p-0"></label>
			<input asp-for= "Name" class="form-control"/>
			</div>
			<div class="mb-3 row p-1">
				<label asp-for="DisplayOrder" class="p-0"></label>
				<input asp-for="DisplayOrder" class="form-control" />
			</div>
		