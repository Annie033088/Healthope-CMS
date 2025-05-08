export default function (mock) {
    const members = [
        {
            MemberId: 1,
            Name: "王小明",
            Phone: "0912345678",
            MembershipExpiry: "2025-12-01T00:00:00",
            Status: true,
            AbsenceTime: 2,
            Email: "",
            AllowGroupClass: "2025-06-01T00:00:00",
            PhoneVerified: true
        },
        {
            MemberId: 2,
            Name: "林小華",
            Phone: "0987654321",
            MembershipExpiry: "2025-06-15T00:00:00",
            Status: false,
            AbsenceTime: 0,
            Email: "",
            AllowGroupClass: "2025-07-01T00:00:00",
            PhoneVerified: false
        },
        {
            MemberId: 3,
            Name: "張偉",
            Phone: "0955111222",
            MembershipExpiry: "2025-11-30T00:00:00",
            Status: true,
            AbsenceTime: 1,
            Email: "wei@example.com",
            AllowGroupClass: "2025-06-05T00:00:00",
            PhoneVerified: true
        },
        {
            MemberId: 4,
            Name: "陳美麗",
            Phone: "0966333444",
            MembershipExpiry: "2025-09-01T00:00:00",
            Status: true,
            AbsenceTime: 0,
            Email: "",
            AllowGroupClass: "2025-06-10T00:00:00",
            PhoneVerified: false
        },
        {
            MemberId: 5,
            Name: "李志強",
            Phone: "0933222111",
            MembershipExpiry: "2026-01-01T00:00:00",
            Status: true,
            AbsenceTime: 3,
            Email: "li@example.com",
            AllowGroupClass: "2025-06-03T00:00:00",
            PhoneVerified: true
        },
        {
            MemberId: 6,
            Name: "吳小美",
            Phone: "0922666888",
            MembershipExpiry: "2025-05-31T00:00:00",
            Status: false,
            AbsenceTime: 4,
            Email: "",
            AllowGroupClass: "2025-06-20T00:00:00",
            PhoneVerified: false
        },
        {
            MemberId: 7,
            Name: "周文豪",
            Phone: "0911888999",
            MembershipExpiry: "2025-10-15T00:00:00",
            Status: true,
            AbsenceTime: 1,
            Email: "wenhao@example.com",
            AllowGroupClass: "2025-06-07T00:00:00",
            PhoneVerified: true
        },
        {
            MemberId: 8,
            Name: "鄭大同",
            Phone: "0977555333",
            MembershipExpiry: "2025-08-01T00:00:00",
            Status: false,
            AbsenceTime: 2,
            Email: "",
            AllowGroupClass: "2025-07-01T00:00:00",
            PhoneVerified: false
        },
        {
            MemberId: 9,
            Name: "趙天宇",
            Phone: "0939444777",
            MembershipExpiry: "2025-07-01T00:00:00",
            Status: true,
            AbsenceTime: 0,
            Email: "tianyu@example.com",
            AllowGroupClass: "2025-06-18T00:00:00",
            PhoneVerified: true
        },
        {
            MemberId: 10,
            Name: "曾雅文",
            Phone: "0981999000",
            MembershipExpiry: "2026-03-01T00:00:00",
            Status: true,
            AbsenceTime: 5,
            Email: "yawan@example.com",
            AllowGroupClass: "2025-06-12T00:00:00",
            PhoneVerified: true
        },
        {
            MemberId: 11,
            Name: "馮思涵",
            Phone: "0900222444",
            MembershipExpiry: "2025-12-01T00:00:00",
            Status: false,
            AbsenceTime: 2,
            Email: "",
            AllowGroupClass: "2025-06-30T00:00:00",
            PhoneVerified: false
        }
    ]

    mock.onPost("/api/Member/GetPermission").reply(() => {
        return [200, { ErrorCode: 1, ApiDataObject: [1, 2] }]
    })

    mock.onPost("/api/Member/GetMemberById").reply(config => {
        let getAdminByIdDto = JSON.parse(config.data);
        let adminTarget = members.find(admin => admin.AdminId === Number(getAdminByIdDto.AdminId));

        if (adminTarget) {
            return [200, { ErrorCode: 1, ApiDataObject: adminTarget }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/Member/EditMember").reply(() => {
        return [200, { ErrorCode: 14 }]
    })

    mock.onPost("/api/Member/GetMember").reply(config => {
        let {
            Status,
            SortOrder,
            SortOption,
            RecordPerPage,
            SearchName,
            SearchPhone,
            Page
        } = JSON.parse(config.data);

        Status = Status === "true" ? true : Status;
        Status = Status === "false" ? false : Status;

        // 1️⃣ 篩選
        let filtered = members.filter(item => {
            const matchStatus = Status === null || item.Status === Status;
            const matchName = !SearchName || item.Name.includes(SearchName);
            const matchPhone = !SearchPhone || item.Phone.slice(-3) === SearchPhone;
            return matchStatus && matchName && matchPhone;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "name") {
            field = "Name";
        } else if (SortOption === "status") {
            field = "Status";
        }
        else if (SortOption === "membershipExpiry") {
            field = "MembershipExpiry"
        } else {
            field = "MemberId"
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
        const pageMemberData = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            MemberList: pageMemberData,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    mock.onPost("/api/Member/DeleteMember").reply(config => {
        let adminIdDto = JSON.parse(config.data);
        const index = members.findIndex(admin => admin.AdminId === Number(adminIdDto.AdminId));

        if (index !== -1) {
            members.splice(index, 1); // 從陣列中移除那個 member
            return [200, { ErrorCode: 1 }]
        }

        return [200, { ErrorCode: 12 }]
    })
}