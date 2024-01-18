# Auth/认证

## Grant token/获取令牌

> POST /api/identity/grant

- Request body/请求体

| Parameter/参数 | Type/类型 | Description/描述 | Required/必填 |
|--------------|---------|----------------|-------------|
| UserName     | string  | User name/用户名  | true        |
| Password     | string  | Password/密码    | true        |

- Response body/响应体

| Parameter/参数 | Type/类型 | Description/描述     |
|--------------|---------|--------------------|
| AccessToken  | string  | Access token/访问令牌  |
| RefreshToken | string  | Refresh token/刷新令牌 |
| TokenType    | string  | Token type/令牌类型    |
| ExpiresIn    | long    | Expires in/过期时间    |

## Refresh token/刷新令牌

> POST /api/identity/refresh

- Query parameters/查询参数

| Parameter/参数 | Type/类型 | Description/描述     | Required/必填 |   
|--------------|---------|--------------------|-------------|
| token        | string  | Refresh token/刷新令牌 | true        |

- Response body/响应体

| Parameter/参数 | Type/类型 | Description/描述     |
|--------------|---------|--------------------|
| AccessToken  | string  | Access token/访问令牌  |
| RefreshToken | string  | Refresh token/刷新令牌 |
| TokenType    | string  | Token type/令牌类型    |
| ExpiresIn    | long    | Expires in/过期时间    |



# User/用户

## Get user/获取用户