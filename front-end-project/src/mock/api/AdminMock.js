export default function (mock) {
    mock.onPost("/api/Admin/AddAdmin").reply(config => {
        const addAdminDto = JSON.parse(config.data);
        const regex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/;

        if (regex.test(addAdminDto.Account) && regex.test(addAdminDto.Pwd) &&
        addAdminDto.Account !== addAdminDto.Pwd) {
            return [200, { ErrorCode: 1 }];
        } else {
            return [200, { ErrorCode: 10 }];
        }
    })

    mock.onPost("/api/Admin/GetPermission").reply(() => {
        return [200, { ErrorCode: 1, ApiDataObject: [1,2] }]
    })
}