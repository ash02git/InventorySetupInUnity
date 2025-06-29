Corrections made:- 
1. Implemented observer pattern for events such as OnItemSold, OnItemBought, OnItemClicked, etc.
2. Changed Code Architecture:- Service Locator Design pattern used. GameService as singleton and ShopService, InventoryService, PopupService, etc as services.
3. Shop MVC, InventoryMVC and Item MVC( Item MVC's model is ItemScriptableObject).
4. Event Service for Implementing Observer Pattern.
