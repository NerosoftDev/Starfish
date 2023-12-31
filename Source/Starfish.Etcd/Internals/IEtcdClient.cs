﻿using Google.Protobuf;

namespace Nerosoft.Starfish.Etcd;

internal interface IEtcdClient
{
	AuthDisableResponse AuthDisable(AuthDisableRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthDisableResponse> AuthDisableAsync(AuthDisableRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthEnableResponse AuthEnable(AuthEnableRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthEnableResponse> AuthEnableAsync(AuthEnableRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthenticateResponse Authenticate(AuthenticateRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	CompactionResponse Compact(CompactionRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<CompactionResponse> CompactAsync(CompactionRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	DeleteRangeResponse Delete(DeleteRangeRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	DeleteRangeResponse Delete(string key, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<DeleteRangeResponse> DeleteAsync(DeleteRangeRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<DeleteRangeResponse> DeleteAsync(string key, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	DeleteRangeResponse DeleteRange(string prefixKey, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<DeleteRangeResponse> DeleteRangeAsync(string prefixKey, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void Dispose();
	RangeResponse Get(RangeRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	RangeResponse Get(string key, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<RangeResponse> GetAsync(RangeRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<RangeResponse> GetAsync(string key, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	RangeResponse GetRange(string prefixKey, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<RangeResponse> GetRangeAsync(string prefixKey, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	IDictionary<string, string> GetRangeVal(string prefixKey, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<IDictionary<string, string>> GetRangeValAsync(string prefixKey, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	string GetVal(string key, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<string> GetValAsync(string key, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	PutResponse Put(PutRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	PutResponse Put(string key, string val, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<PutResponse> PutAsync(PutRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<PutResponse> PutAsync(string key, string val, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthRoleAddResponse RoleAdd(AuthRoleAddRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthRoleAddResponse> RoleAddAsync(AuthRoleAddRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthRoleDeleteResponse RoleDelete(AuthRoleDeleteRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthRoleDeleteResponse> RoleDeleteAsync(AuthRoleDeleteRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthRoleGetResponse RoleGet(AuthRoleGetRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthRoleGetResponse> RoleGetAsync(AuthRoleGetRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthRoleGrantPermissionResponse RoleGrantPermission(AuthRoleGrantPermissionRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthRoleGrantPermissionResponse> RoleGrantPermissionAsync(AuthRoleGrantPermissionRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthRoleListResponse RoleList(AuthRoleListRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthRoleListResponse> RoleListAsync(AuthRoleListRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthRoleRevokePermissionResponse RoleRevokePermission(AuthRoleRevokePermissionRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthRoleRevokePermissionResponse> RoleRevokePermissionAsync(AuthRoleRevokePermissionRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	TxnResponse Transaction(TxnRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<TxnResponse> TransactionAsync(TxnRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthUserAddResponse UserAdd(AuthUserAddRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthUserAddResponse> UserAddAsync(AuthUserAddRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthUserChangePasswordResponse UserChangePassword(AuthUserChangePasswordRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthUserChangePasswordResponse> UserChangePasswordAsync(AuthUserChangePasswordRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthUserDeleteResponse UserDelete(AuthUserDeleteRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthUserDeleteResponse> UserDeleteAsync(AuthUserDeleteRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthUserGetResponse UserGet(AuthUserGetRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthUserGetResponse> UserGetAsync(AuthUserGetRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthUserGrantRoleResponse UserGrantRole(AuthUserGrantRoleRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthUserGrantRoleResponse> UserGrantRoleAsync(AuthUserGrantRoleRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthUserListResponse UserList(AuthUserListRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthUserListResponse> UserListAsync(AuthUserListRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	AuthUserRevokeRoleResponse UserRevokeRole(AuthUserRevokeRoleRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task<AuthUserRevokeRoleResponse> UserRevokeRoleAsync(AuthUserRevokeRoleRequest request, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void Watch(string key, Action<WatchEvent[]> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void Watch(string key, Action<WatchEvent[]>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void Watch(string key, Action<WatchResponse> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void Watch(string key, Action<WatchResponse>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void Watch(string[] keys, Action<WatchEvent[]> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void Watch(string[] keys, Action<WatchEvent[]>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void Watch(string[] keys, Action<WatchResponse> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void Watch(string[] keys, Action<WatchResponse>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(string key, Action<WatchEvent[]> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(string key, Action<WatchEvent[]>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(string key, Action<WatchResponse> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(string key, Action<WatchResponse>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(string[] keys, Action<WatchEvent[]> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(string[] keys, Action<WatchEvent[]>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(string[] keys, Action<WatchResponse> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(string[] keys, Action<WatchResponse>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(WatchRequest request, Action<WatchEvent[]> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(WatchRequest request, Action<WatchEvent[]>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(WatchRequest request, Action<WatchResponse> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(WatchRequest request, Action<WatchResponse>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(WatchRequest[] requests, Action<WatchEvent[]> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(WatchRequest[] requests, Action<WatchEvent[]>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(WatchRequest[] requests, Action<WatchResponse> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchAsync(WatchRequest[] requests, Action<WatchResponse>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void WatchRange(string path, Action<WatchEvent[]> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void WatchRange(string path, Action<WatchEvent[]>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void WatchRange(string path, Action<WatchResponse> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void WatchRange(string path, Action<WatchResponse>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void WatchRange(string[] paths, Action<WatchEvent[]> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void WatchRange(string[] paths, Action<WatchEvent[]>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void WatchRange(string[] paths, Action<WatchResponse> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	void WatchRange(string[] paths, Action<WatchResponse>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchRangeAsync(string path, Action<WatchEvent[]> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchRangeAsync(string path, Action<WatchEvent[]>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchRangeAsync(string path, Action<WatchResponse> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchRangeAsync(string path, Action<WatchResponse>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchRangeAsync(string[] paths, Action<WatchEvent[]> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchRangeAsync(string[] paths, Action<WatchEvent[]>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchRangeAsync(string[] paths, Action<WatchResponse> method, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
	Task WatchRangeAsync(string[] paths, Action<WatchResponse>[] methods, Grpc.Core.Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
}
