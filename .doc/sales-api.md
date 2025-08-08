[Back to README](../README.md)

### Sales

#### GET /api/sales
- Description: Retrieve a list of all sales with optional filtering
- Query Parameters:
  - `customerId` (optional): Filter sales by customer ID
  - `branchId` (optional): Filter sales by branch ID
  - `startDate` (optional): Filter sales by start date
  - `endDate` (optional): Filter sales by end date
- Response: 
  ```json
  {
    "success": true,
    "message": "Sales retrieved successfully",
    "data": {
      "sales": [
        {
          "id": "guid",
          "saleNumber": "string",
          "saleDate": "datetime",
          "customerId": "guid",
          "customerName": "string",
          "branchId": "guid",
          "branchName": "string",
          "totalAmount": "decimal",
          "status": "string (enum: Active, Cancelled)",
          "itemCount": "integer"
        }
      ]
    }
  }
  ```

#### POST /api/sales
- Description: Create a new sale
- Request Body:
  ```json
  {
    "customerId": "guid",
    "customerName": "string",
    "branchId": "guid",
    "branchName": "string",
    "items": [
      {
        "productId": "guid",
        "productName": "string",
        "quantity": "integer",
        "unitPrice": "decimal"
      }
    ]
  }
  ```
- Response: 
  ```json
  {
    "success": true,
    "message": "Sale created successfully",
    "data": {
      "id": "guid",
      "saleNumber": "string",
      "saleDate": "datetime",
      "customerId": "guid",
      "customerName": "string",
      "branchId": "guid",
      "branchName": "string",
      "totalAmount": "decimal",
      "items": [
        {
          "id": "guid",
          "productId": "guid",
          "productName": "string",
          "quantity": "integer",
          "unitPrice": "decimal",
          "discountPercentage": "decimal",
          "totalAmount": "decimal"
        }
      ]
    }
  }
  ```

#### GET /api/sales/{id}
- Description: Retrieve a specific sale by ID
- Path Parameters:
  - `id`: Sale ID (GUID)
- Response: 
  ```json
  {
    "success": true,
    "message": "Sale retrieved successfully",
    "data": {
      "id": "guid",
      "saleNumber": "string",
      "saleDate": "datetime",
      "customerId": "guid",
      "customerName": "string",
      "branchId": "guid",
      "branchName": "string",
      "totalAmount": "decimal",
      "status": "string (enum: Active, Cancelled)",
      "createdAt": "datetime",
      "updatedAt": "datetime",
      "items": [
        {
          "id": "guid",
          "productId": "guid",
          "productName": "string",
          "quantity": "integer",
          "unitPrice": "decimal",
          "discountPercentage": "decimal",
          "totalAmount": "decimal",
          "isCancelled": "boolean"
        }
      ]
    }
  }
  ```

#### PUT /api/sales/{id}
- Description: Update a specific sale
- Path Parameters:
  - `id`: Sale ID (GUID)
- Request Body:
  ```json
  {
    "customerId": "guid",
    "customerName": "string",
    "branchId": "guid",
    "branchName": "string",
    "items": [
      {
        "productId": "guid",
        "productName": "string",
        "quantity": "integer",
        "unitPrice": "decimal"
      }
    ]
  }
  ```
- Response: 
  ```json
  {
    "success": true,
    "message": "Sale updated successfully",
    "data": {
      "id": "guid",
      "saleNumber": "string",
      "saleDate": "datetime",
      "customerId": "guid",
      "customerName": "string",
      "branchId": "guid",
      "branchName": "string",
      "totalAmount": "decimal",
      "status": "string (enum: Active, Cancelled)",
      "createdAt": "datetime",
      "updatedAt": "datetime",
      "items": [
        {
          "id": "guid",
          "productId": "guid",
          "productName": "string",
          "quantity": "integer",
          "unitPrice": "decimal",
          "discountPercentage": "decimal",
          "totalAmount": "decimal",
          "isCancelled": "boolean"
        }
      ]
    }
  }
  ```

#### DELETE /api/sales/{id}
- Description: Delete or cancel a specific sale
- Path Parameters:
  - `id`: Sale ID (GUID)
- Query Parameters:
  - `canDelete` (optional): If true, physically deletes the record; if false (default), marks it as cancelled
- Response: 
  ```json
  {
    "success": true,
    "message": "Sale cancelled successfully" // or "Sale permanently deleted successfully"
  }
  ```

<br/>
<div style="display: flex; justify-content: space-between;">
  <a href="./products-api.md">Previous: Products API</a>
  <a href="./carts-api.md">Next: Carts API</a>
</div>