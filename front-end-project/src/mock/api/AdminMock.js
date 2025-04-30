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
        return [200, { ErrorCode: 1, ApiDataObject: [1, 2] }]
    })

    mock.onPost("/api/Admin/GetAdminById").reply(config => {
        let getAdminByIdDto = JSON.parse(config.data);
        let adminTarget = AdminList.find(admin => admin.AdminId === getAdminByIdDto.AdminId);
        if (adminTarget) {
            return [200, { ErrorCode: 1, ApiDataObject: adminTarget }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/Admin/EditAdmin").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Admin/GetAdmin").reply(config => {
        let {
            Status,
            SortOrder,
            SortOption,
            RecordPerPage,
            SearchAccount,
            Page
        } = JSON.parse(config.data);

        Status = Status === "true" ? true : Status;
        Status = Status === "false" ? false : Status;

        // 1️⃣ 篩選
        let filtered = AdminList.filter(item => {
            const matchStatus = Status === null || item.Status === Status;
            const matchSearch = !SearchAccount || item.Account.includes(SearchAccount);
            return matchStatus && matchSearch;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "account") {
            field = "Account";
        } else if (SortOption === "status") {
            field = "Status";
        }
        else {
            field = "AdminId"
        }

        filtered.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (SortOption === 'status') {
                aVal = aVal ? 1 : 0;
                bVal = bVal ? 1 : 0;
            }

            if (aVal < bVal) return SortOrder === 'descending' ? 1 : -1;
            if (aVal > bVal) return SortOrder === 'descending' ? -1 : 1;
            return 0;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const paged = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            AdminList: paged,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    const AdminList = [
        {
            AdminId: 1,
            Account: "qweokqwp21312",
            Status: true,
            Identity: 3,
            UpdateTime: "2025-04-29T12:34:56.789Z"
        },
        {
            AdminId: 2,
            Account: "generalAdmin123",
            Status: true,
            Identity: 2,
            UpdateTime: "2025-04-25T12:34:56.789Z"
        },
        {
            AdminId: 3,
            Account: "disabledAdmin456",
            Status: false,
            Identity: 2,
            UpdateTime: "2025-04-20T08:00:00.000Z"
        },
        {
            AdminId: 4,
            Account: "adminTester001",
            Status: true,
            Identity: 7,
            UpdateTime: "2025-03-15T09:12:00.000Z"
        },
        {
            AdminId: 5,
            Account: "adminAlphaUser35",
            Status: false,
            Identity: 2,
            UpdateTime: "2025-03-10T17:45:00.000Z"
        },
        {
            AdminId: 6,
            Account: "mainAlphaUser71",
            Status: true,
            Identity: 3,
            UpdateTime: "2025-04-01T11:00:00.000Z"
        },
        {
            AdminId: 7,
            Account: "tempCoachAdminX5",
            Status: false,
            Identity: 4,
            UpdateTime: "2025-02-18T14:00:00.000Z"
        },
        {
            AdminId: 8,
            Account: "userAccountantY1",
            Status: true,
            Identity: 2,
            UpdateTime: "2025-04-05T10:00:00.000Z"
        },
        {
            AdminId: 9,
            Account: "alphaUserAdmin2",
            Status: false,
            Identity: 6,
            UpdateTime: "2025-01-28T19:00:00.000Z"
        },
        {
            AdminId: 10,
            Account: "rootManagerZ9",
            Status: true,
            Identity: 3,
            UpdateTime: "2025-03-22T21:30:00.000Z"
        }
    ]
}