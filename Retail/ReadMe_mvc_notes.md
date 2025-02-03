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
		[HttpPost] Method implementation
		24. Adding HttpPost Method Attribute in CategoryController
			[HttpPost]
			[HttpPost]
			 public IActionResult Create(Category obj)
				{
				_db.Categories.Add(obj);    // Keeping track of what needs to Add
				_db.SaveChanges();          // Creates a Category in Db
				return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method
				}
		SEVER-SIDE VALIDATION
		25. SeversSide Validation in CategoryController.cs	
			a. First add validation in the properyt in MODEL Category.cs 
				 public class Category
				{
					[Key]   
					public int Id { get; set; }
					[Required]
					[MaxLength(30)] //Length Validation
					[DisplayName("Category Name")]
					public string Name { get; set; }

					[DisplayName("Display Order")]
					[Range(1,100, ErrorMessage ="Dispaly Order must be between 1-100")] // DisplayOrder Range Validation
					public int DisplayOrder { get; set; }
				}
			b. Add VALIDATION MEDTOD in CONTROLLER CategoryController
			
				[HttpPost]
				public IActionResult Create(Category obj)
				{
					if (ModelState.IsValid)
					{
						_db.Categories.Add(obj);    // Keeping track of what needs to Add
						_db.SaveChanges();          // Creates a Category in Db
						return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method
					}
					return View();

				}
			c. Add TAG-HELPER in VIEWS of Category view Create.cs
				<div class="mb-3 row p-1">
					<label asp-for= "Name" class="p-0"></label>
					<input asp-for= "Name" class="form-control"/>
					<span asp-validation-for="Name" class="text-danger"></span>
				</div>
				<div class="mb-3 row p-1">
					<label asp-for="DisplayOrder" class="p-0"></label>
					<input asp-for="DisplayOrder" class="form-control" />
					<span asp-validation-for="DisplayOrder" class="text-danger"></span>
				</div>
		CUSTOM VALIDATION ERROR MESSAGE
		26. Add VALIDATION in CONTROLLER CategoryController
		1.
		 [HttpPost]
         public IActionResult Create(Category obj)
         {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
		2. ERROR MESSAGE SUMMARY
			Add in VIEWS when Error message Summary is required
			<div asp-validation-summary="All" class="text-bg-danger"></div>
			asp-validation-summary other options are:
			ModelOnly, None, and All
		
		CLIENT SIDE VALIDATION
		27. User EF ValidationScriptPartial for Client Side VALIDATION
			Use the Builtin snippet available in EF.
			@section Scripts{
				{
					<partial name="_ValidationScriptsPartial" /> 
				} 
			}
		
		GET CATEGORY EDIT MODEL
		28. ADD IActionResult METHOD IN CategoryController
			1.
			Adding a GE METHOD (Get Method is available by Default)
				public IActionResult Edit(int? id)
				{
					if (id == null || id == 0)
					{
						return NotFound();
						//Add an Error page for Enduser
					}
					Category? categoryFromDb = _db.Categories.Find(id); // Method-1: By ID only
					// Method-2:First or Default Method
					//Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
					// Method-3:Link Operation
					//Category? categoryFromDb2 = _db.Categories.Where( u => u.Id == id).FirstOrDefault(); 
					if (categoryFromDb == null)
					{
						return NotFound();
					}
					return View(categoryFromDb);
				}
			[HttpPost]
			 public IActionResult Edit(Category obj)
			{

				return View();

			}
			2. Retrive Category ID from Category INDEX View
			<div class="w-75 btn-group" role="group">
							<a asp-controller="Category" asp-action="Edit" asp-route-id = "@obj.Id" class="btn btn-primary mx-2">
							<i class="bi bi-pencil-square"></i> Edit
							</a>
		
		GET CATEGORY EDIT View
		29. Adding Edit View page from CategoryController
			1. Right Click on IActionResult Edit (Select Razor View)
			2. Copy the content of Create View and Change the create to Edit and Update
		
		DELETE CATEGORY LIST from MODEL CategoryController
		30. 
			1. Add Delete IActionResult in CategoryController
			2. Add A Delete View page
			4. Add asp-route-id for Delete Action in Index View
		
		TEMPDATA TO DISPLAY ADD,UPDATE, & DELETE VALUES
		31. 
		1. Add TempData Operation in CategoryController
		
		if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);    // Keeping track of what needs to Add
                _db.SaveChanges();          // Creates a Category in Db
                TempData["success"] = "Category Created successfully";
                return RedirectToAction("Index");  //Redirect to "Index" through IActionResult Method
            }
            return View();
		2. To Display TempData["success"] add it to Index View.
		@model List<Category>
		// Add the following
		@if (TempData["success"] != null)
		{
			<h2>@TempData["success"]</h2>
		}
		
		ADDING THE TEMPDATA IN MULTIPLE Views
		32. 
			1. Create Partial View in Shared Folder with "_" undescore
			2. Add the reusable code in shared View
				@if (TempData["success"] != null)
				{
					<h4>@TempData["success"]</h4>
				}

				@if (TempData["error"] != null)
				{
					<h4>@TempData["error"]</h4>
				}
			3. Add the partial view usage in INDEX file
			
				@model List<Category>

				<partial name="_Notification"/>
		
		33. ADDING FANCY LOOK TO NOTICFICATION
			1. Go to Toastr site and get cdn link for css and js
			2. Paste css in _Layout page
			3. Add the Toastr code in _Notification page
				@if (TempData["success"] != null)
				{
					<script src="~/lib/jquery/dist/jquery.min.js"></script>
					<script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
					<script type="text/javascript">
						toastr.success('@TempData["success"]')
					</script>
				}

				@if (TempData["error"] != null)
				{
					<script src="~/lib/jquery/dist/jquery.min.js"></script>
					<script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
					<script type="text/javascript">
						toastr.success('@TempData["success"]')
					</script>
				}
			4. 
		34.