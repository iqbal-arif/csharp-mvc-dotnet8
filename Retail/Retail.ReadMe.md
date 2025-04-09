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
	
ADDING APPLICATION User
47. Modify IdentityUser to ApplicationUser in Registre.cshtml to create Application User
	 private ApplicationUser CreateUser()
	 {
     try
     {
         return Activator.CreateInstance<ApplicationUser>();
     }
     catch
	 
Adding USER ROLES
48. Adding Identity Role in Program.cs in replaciing the AddDefaultIdentity
      builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
	  
	  modifying to add AddIdentity with IdentityRole LOAD
	  
	  builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

49. Adding Varialbe Role Manager in Register Model
        
	  public readonly RoleManager<IdentityRole> _roleManager;
	  
50. ADDING ROLE USING Dependency Injection in Register.cs

	        UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, //Using Dependency Injector
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
			{
				_roleManager = roleManager;
				_userManager = userManager;
51. Creating Role in Database after checking its existance in Get Handler in Register.cs	 
	 
	 public async Task OnGetAsync(string returnUrl = null)
		{
		if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
		{
			_roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
			_roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
			_roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
			_roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
		}

52. Creating Constant for Roles in RetailWeb.Utility SD.cs

	namespace Retail.Utility
	{
		public static class SD
		{
			public const string Role_Customer = "Customer";
			public const string Role_Company = "Company";
			public const string Role_Admin = "Admin";
			public const string Role_Employee = "Employee";
		}
	}
       
53. Add EmailSender Implementaion in Reatil.Utility as EmailSender.cs

		public class EmailSender : IEmailSender
		{
			public Task SendEmailAsync(string email, string subject, string htmlMessage)
				{
				//Logic to Send Email
				return Task.CompletedTask;
				}
		}
54. Adding Service for EmailSender in Program.cs
            builder.Services.AddScoped<IEmailSender, EmailSender>(); /// Implementation of EmailSender

ADDING ROLE SELETION IN REGISTRAION PAGE
55. Add Role Property in Register.cs for RegisterModels
	//ROLE Property
	public string? Role { get; set; }
56. Add Role SelectList
	[ValidateNever]
	public IEnumerable<SelectListItem> RoleList { get; set; }	
	
57. Add Select list in Register View PAGE
	<div class="form-floating mb-3">
		<select asp-for="Input.Role" asp-items="@Model.Input.RoleList" class="form-select" >
			<option disabled selected> - Select Role - </option>
		</select>
	</div>
	NOTE: "asp-for" uses @model by Default, but for "asp-items" @Model IS NEED TO BE SPECIFIED.
	
ASSIGNED ROLE ONPOST
58. Assign Role ONPOST in Register.cs
	if (!String.IsNullOrEmpty(Input.Role))
	{
		await _userManager.AddToRoleAsync(user, Input.Role);
	}
	else
	{
		await _userManager.AddToRoleAsync(user, SD.Role_Customer); // SD IS STATIC DETAILS
	}
59. Add DefaultTokenProvider FOR Add IDENTITY in Program.cs Service Registery

	builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
60. ADDING AUTORIZAITON FOR Admin
	1. Adding "@using Retail.Utility" in ALL "_ViewImports.cshtml, under Area/Admin/Views; Areas/Customer/Views; RetailWeb/Views/Shared and finally Identity/Pages
	2. Adding Authorization Condition for Admin Elements in _Layout.cshtml
		 @if(User.IsInRole(SD.Role_Admin))
                            {
                                <li class="nav-item dropdown">
                                    <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                        Content Management
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li class="nav-item">
                                            <a class="dropdown-item" asp-area="Admin" asp-controller="Category" asp-action="Index">Category</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="dropdown-item" asp-area="Admin" asp-controller="Product" asp-action="Index">Product</a>
                                        </li>
                                        <li><hr class="dropdown-divider"></li>
                                    </ul>
                                </li>
                            }
    
	3. Add [Authorize(Roles =SD.Role_Admin)]in CategoryController and ProductController to avoid URL access to those pages.
61. REDIRECT TO ACCESS DENIED PAGES
	Add the Services in Program.cs after IDENTITY CORE Service
	//Access Denied Page Redirect
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LoginPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

62. ADDING ATTRIBUTES IN REGISTER.cs
		//Adding Fields that are added through in DB in Identity Core Installation
            public string? StreetAddress { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? PostalCode { get; set; }
            public string? PhoneNumber { get; set; }
63. ADDING USER PROPERTIES IN CREATING A NEW User In REGISTER.cshtml.cs
    1. Adding Properties to be created in OnPostAsynce for CreateUser() function as follows;
		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.StreetAddress = Input.StreetAddress;
                user.City = Input.City;
                user.Name = Input.Name;
                user.State = Input.State;
                user.PostalCode = Input.PostalCode;
                user.PhoneNumber = Input.PhoneNumber;
                var result = await _userManager.CreateAsync(user, Input.Password);

    2. Adding Fileds for the Properties in REGISTER.cshtml.cs
	   
	   public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
             .
			 .
			 .
			 .
			 .
            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }

            //Adding Fields that are added through in DB in Identity Core Installation
            [Required]
            public string? Name { get; set; }
            public string? StreetAddress { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? PostalCode { get; set; }
            public string? PhoneNumber { get; set; }

        }
	3. Modify in ApplicationUser.cs The Name Type Int to String
	4. Run add-migration UpdateNameToBeStringApplicaitonUser To Update the Type in Db
	5.Uupdate-Database

64. ADDING COMPANY Role
	
	1. Create Company.cs Model under Retail.Models with all the parameters
		namespace Retail.Models
		{
			public class Company
			{
				public int Id { get; set; }
				[Required]
				public string? Name { get; set; }
				public string? StreetAddress { get; set; }
				public string? City { get; set; }
				public string? State { get; set; }
				public string? PostalCode { get; set; }
				public string? PhoneNumber { get; set; }
			}
		}
	2. Add it to DbSet in ApplicationDbContext
		// Creates DataBase Table name as "Company"
			public DbSet<Company> Companies { get; set; }
	3. Updating the Database 
		a. add-migration addCompanyTable  Default Project: Retail.DataAccess
		b. Update-Database
	4. Create CompanyRepository
	5. Create ICompanyRepository
	6. Add ICompanyRepository to IUnitOfWork
	7. Add ICompanyRepository field and create of Comapny instance in UnitOfWork
	8. Create CompanyController.cs under Areas/Admin/Controllers
		To open Find and Replace widget Ctrl+Shift+F
	9. Creat Company View (Add Index.cshmtl, Add Upsert.cshmtl, Add company.js)
	10. Add the Company Menu in the Naviation Menu in _Layout.cshtml
	11. Add Comapny Seed in AplicationDbContext.cs FILE
	12. add-migration addCompanyRecords
	13. update-database
	14. Creating Reationship between User and Company
		14.1. Add CompanyId variable in ApplicationUser.cs
				public string? CompanyId { get; set; } // Foreingn Key Relationship for the USER with COMPANY
				[ForeignKey("CompanyId")]
				[ValidateNever]
				public Company Company { get; set; }
		14.2 add-migration addCompanyToUser
		14.3 update-database
		14.4 Adding a List of Company in The Registration Page; Populate the register dropdown and pass it Razor Page.
			public RegisterModel(.....IUnitOfWork unitOfWork)
			{_unitOfWork = unitOfWork;}
		14.5 Add CompanyId into the Model Register.cshmtl
			A. //Adding Fields that are added through in DB in Identity Core Installation
			public int? CompanyId { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> CompanyList { get; set; }
			B. //Adding CompnayList
			Input = new()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem{
                    Text = i,
                    Value = i
                }),
                CompanyList = _unitOfWork.Compnay.GetAll().Select(i => new SelectListItem{
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
			C. Add JavaScript for Toggle CompanyList if Role is company
			D. Add Particular Company Id When User Register as Company and select a particular company.
				if(Input.Role == SD.Role_Company)
                {
                    user.CompanyId = Input.CompanyId;
                }
	
65. ADDING SHOPPINGCART Model in Retail.ModelState
66. ADDING Db for SHOPPINGCART in AplicationDbContext
67. add-migration addShoppingCartToDb
68. CREATE IRepository, Repository for SHOPPINGCART AND Add to UnitOfWork
69. CREATE IRepository, Repository for ApplicationUser AND Add to UnitOfWork
70. Modify Customer Detial View to reflect Product and its Count in Areas/Customer/Views/Home/Details.cshmtl
71. ADD SHOPPINGCART VIEW in Customer/Controller/HomeController.cs file.
	 public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(detail => detail.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }
72	ADD TO CART:
	72.1: Add a hidden filed in Customer Detail page to get Product id That will be needed to upadte or enter the product.
		<form method="post">
		@* Hidden Id will be used or populated with Product is updated in the shoppinngcart.  *@
		<input hidden asp-for= "ProductId" /> 
	72.2: Getting User ID in HomeController that will be populated during Login process.
		//To Get User Id through Helper method ClaimsIdentity
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
73	ADD ORDER CONFIRMATION: 
	73.1: add-migration addOrderHeaderAndDetailsToDb

74. ADD ORDER HEADER & DETAIL TO Repository

	74.1: Add IOrderHeaderRepository & IOrderDetailRepository
	74.2: Add Repository Implementaion OrderHeaderRepository & OrderDetailRepository
	74.3: Add the IOrderHeaderRepository & IOrderDetailRepository to IUnitOfWork
	74.4: Add the IOrderHeaderRepository & IOrderDetailRepository to UnitOfWork and Initialize them.
	74.5: Adding OrderHeader in the ShoppingCartVm
			public OrderHeader OrderHeader { get; set; }
75. SUMMARY GET ACTION METHOD
	75.1: Populating ShoppingCartVm.OrderHeader.ApplicationUser Details
	
	        public IActionResult Summery()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                    includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;


            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }
76......Fill the topic namespace

77. STRIPE SETUP
	77.1. Setup Stripe Account
	77.2. Set Public and Secret Key in appsettings.JSON
	77.3. Write StripeSettings.cs class in Retail.Utility and set Two Key Properties.
	77.4. At RetailWeb level add Stripe module through Nuget Package.
	77.5. Configuring the API-Key to bring the Stripe Secret Keys
			 //Injecting Stripe Keys into StripeSettings.cs
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
			//Stripe API Key Configuration
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
	77.6. add-migration addSessionIdToOrderHeader
	77.7 Custom Helper METHOD to set order status and payment status. Create UpdateStatus in IOrderHeaderRepository
			void UpdateStatus(int id,string orderStatus, string? paymentStatus = null);
			void UpdateStripePaymentID(int id,string sessionId, string paymentIntenId);
	77.7. IMPLEMENT the method in OrderHeaderRepository
			//Updating Status
        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            //Retrive OrderHeader from DB based on ID and Update OrderStatus
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderFromDb != null)
            {
                //Order Status
                orderFromDb.OrderStatus = orderStatus;

                //Payment Status
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }

            }
        }
        
        //Payment Intent ID  and Session Id
        //Session Id is generated at payment attempt, upon successful status then Payment Intent Id is generated
        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntenId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);

            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIntenId))
            {
                orderFromDb.PaymentIntentId = paymentIntenId;
                //and update the date of payment 
                orderFromDb.PaymentDate = DateTime.Now;
            }
        }
	77.8: Copy the Stripe CheckOut Session METHOD from Stripe API Documentation
	77.9: Set URL for the Build for now and route it to OrderConfirmation page in CartController.cs
			//Capture Payment for Regular Customer
            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //It is a Regular Customer Account and we need to capture payment
                //Stripe Logic

                //Variable for URL
                var domain = "https://localhost:7113/";
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    //URl = doamin + navigate to Customer Area , Cart Controller, and Action OrderConfirmation
                    SuccessUrl = domain+ $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                    {
                        new Stripe.Checkout.SessionLineItemOptions
                        {
                            Price = "price_1MotwRLkdIwHu7ixYcPLm5uZ",
                            Quantity = 2,
                        },
                    },
                    Mode = "payment",
                };
                var service = new Stripe.Checkout.SessionService();
                service.Create(options);

            }
		
	
		

