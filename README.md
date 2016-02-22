# Masters Thesis - XVP

Sanitized Repository for my masters thesis project:
http://is.muni.cz/th/417457/fi_m/

Title:
Developing a Media Marketplace Multi-Tenant Architecture with Windows Azure and ASP.Net.

Abstract:
The introduction of cloud computing has allowed applications to run on shared resources. As a result, the hosting world has become subject to economies of scale. In order to truly benefit from this shared resource model, many software providers are faced with the question of implementing multi-tenancy. This model contrasts with traditional software approaches, where each grouping of customers are hosted on their own customized application and database instances. Multi-tenancy promotes sharing of resources all the way from a single application instance to the database. This allows applications to better fit the cloud computing paradigm. All the while, providing increased maintainability, scalability and reducing hosting costs. This thesis attempts to create an architecture description that addresses the design concerns related to the implementation of a multi-tenant, media-marketplace that utilizes Microsoft Azure and ASP.NET. A case study is used for providing context and a prototype is developed in order to test the viability of the resulting architecture description. The primary contribution of this thesis is aimed at providing a solid foundation for SaaS providers wishing to implement cloud-native, multi-tenant applications.

To use this project you will need an Azure Subscription with AzureDocumentDb, AzureSearch, Azure WebApp, ApplicationInsights, and AzureRedisCache services setup.

To get started simply replace your connection strings/service keys in the MVC and Webjob Web.config files
