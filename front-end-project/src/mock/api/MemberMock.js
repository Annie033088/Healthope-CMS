export default function (mock) {
    const members = [
    {
        MemberId: 1,
        Name: "王小明",
        Phone: 912345678,
        Email: "",
        BirthDay: "0001-01-01",
        Gender: 0,
        Height: 0,
        Weight: 0,
        Status: true,
        AbsenceTime: 2,
        AllowGroupClass: "2025-06-01T00:00:00",
        MembershipExpiry: "2025-12-01T00:00:00",
        PhoneVerified: true,
        EmergencyContactName: "",
        EmergencyContactPhone: 0,
        EmergencyContactRelation: "",
        CreateTime:"2025-02-01T00:00:00",
    },
    {
        MemberId: 2,
        Name: "陳美麗",
        Phone: 911223344,
        Email: "",
        BirthDay: "0001-01-01",
        Gender: 0,
        Height: 0,
        Weight: 0,
        Status: false,
        AbsenceTime: 5,
        AllowGroupClass: "2025-05-15T00:00:00",
        MembershipExpiry: "2025-10-15T00:00:00",
        PhoneVerified: false,
        EmergencyContactName: "",
        EmergencyContactPhone: 0,
        EmergencyContactRelation: "",
        CreateTime:"2025-02-01T00:00:00",
    },
    {
        MemberId: 3,
        Name: "李大仁",
        Phone: 913334455,
        Email: "",
        BirthDay: "0001-01-01",
        Gender: 0,
        Height: 0,
        Weight: 0,
        Status: true,
        AbsenceTime: 0,
        AllowGroupClass: "2025-07-01T00:00:00",
        MembershipExpiry: "2025-01-31T00:00:00",
        PhoneVerified: true,
        EmergencyContactName: "",
        EmergencyContactPhone: 0,
        EmergencyContactRelation: "",
        CreateTime:"2025-02-01T00:00:00",
    },
    {
        MemberId: 4,
        Name: "張小華",
        Phone: 914556677,
        Email: "",
        BirthDay: "0001-01-01",
        Gender: 0,
        Height: 0,
        Weight: 0,
        Status: true,
        AbsenceTime: 3,
        AllowGroupClass: "2025-06-10T00:00:00",
        MembershipExpiry: "2025-11-10T00:00:00",
        PhoneVerified: false,
        EmergencyContactName: "",
        EmergencyContactPhone: 0,
        EmergencyContactRelation: "",
        CreateTime:"2025-02-01T00:00:00",
    },
    {
        MemberId: 5,
        Name: "林阿忠",
        Phone: 915667788,
        Email: "",
        BirthDay: "0001-01-01",
        Gender: 0,
        Height: 0,
        Weight: 0,
        Status: false,
        AbsenceTime: 1,
        AllowGroupClass: "2025-04-01T00:00:00",
        MembershipExpiry: "2025-09-01T00:00:00",
        PhoneVerified: false,
        EmergencyContactName: "",
        EmergencyContactPhone: 0,
        EmergencyContactRelation: "",
        CreateTime:"2025-02-01T00:00:00",
    },
    {
        MemberId: 6,
        Name: "周玉芬",
        Phone: 916778899,
        Email: "",
        BirthDay: "0001-01-01",
        Gender: 0,
        Height: 0,
        Weight: 0,
        Status: true,
        AbsenceTime: 4,
        AllowGroupClass: "2025-07-15T00:00:00",
        MembershipExpiry: "2025-12-31T00:00:00",
        PhoneVerified: true,
        EmergencyContactName: "",
        EmergencyContactPhone: 0,
        EmergencyContactRelation: "",
        CreateTime:"2025-02-01T00:00:00",
    },
    {
        MemberId: 7,
        Name: "鄭家豪",
        Phone: 917889900,
        Email: "",
        BirthDay: "0001-01-01",
        Gender: 0,
        Height: 0,
        Weight: 0,
        Status: true,
        AbsenceTime: 0,
        AllowGroupClass: "2025-08-01T00:00:00",
        MembershipExpiry: "2026-01-01T00:00:00",
        PhoneVerified: true,
        EmergencyContactName: "",
        EmergencyContactPhone: 0,
        EmergencyContactRelation: "",
        CreateTime:"2025-02-01T00:00:00",
    },
    {
        MemberId: 8,
        Name: "何玉清",
        Phone: 918990011,
        Email: "",
        BirthDay: "0001-01-01",
        Gender: 0,
        Height: 0,
        Weight: 0,
        Status: false,
        AbsenceTime: 6,
        AllowGroupClass: "2025-05-01T00:00:00",
        MembershipExpiry: "2025-09-01T00:00:00",
        PhoneVerified: false,
        EmergencyContactName: "",
        EmergencyContactPhone: 0,
        EmergencyContactRelation: "",
        CreateTime:"2025-02-01T00:00:00",
    },
    {
        MemberId: 9,
        Name: "曾明志",
        Phone: 919101112,
        Email: "",
        BirthDay: "0001-01-01",
        Gender: 0,
        Height: 0,
        Weight: 0,
        Status: true,
        AbsenceTime: 2,
        AllowGroupClass: "2025-06-20T00:00:00",
        MembershipExpiry: "2025-11-20T00:00:00",
        PhoneVerified: true,
        EmergencyContactName: "",
        EmergencyContactPhone: 0,
        EmergencyContactRelation: "",
        CreateTime:"2025-02-01T00:00:00",
    },
    {
        MemberId: 10,
        Name: "賴佩芬",
        Phone: 920112233,
        Email: "",
        BirthDay: "0001-01-01",
        Gender: 0,
        Height: 0,
        Weight: 0,
        Status: true,
        AbsenceTime: 1,
        AllowGroupClass: "2025-06-01T00:00:00",
        MembershipExpiry: "2025-12-01T00:00:00",
        PhoneVerified: true,
        EmergencyContactName: "",
        EmergencyContactPhone: 0,
        EmergencyContactRelation: "",
        CreateTime:"2025-02-01T00:00:00",
    }
];


    mock.onPost("/api/Member/GetMemberEditDataById").reply(config => {
        let memberIdDto = JSON.parse(config.data);
        let memberTarget = members.find(member => member.MemberId === Number(memberIdDto.MemberId));

        if (memberTarget) {
            return [200, { ErrorCode: 1, ApiDataObject: memberTarget }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/Member/GetMemberDetail").reply(config => {
        let memberIdDto = JSON.parse(config.data);
        let memberTarget = members.find(member => member.MemberId === Number(memberIdDto.MemberId));

        if (memberTarget) {
            return [200, { ErrorCode: 1, ApiDataObject: memberTarget }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/Member/EditMember").reply(() => {
        return [200, { ErrorCode: 1 }]
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
        // return [200, { ErrorCode: 1, ApiDataObject }]
    })

    // mock.onPost("/api/Member/DeleteMember").reply(config => {
    //     let adminIdDto = JSON.parse(config.data);
    //     const index = members.findIndex(admin => admin.AdminId === Number(adminIdDto.AdminId));

    //     if (index !== -1) {
    //         members.splice(index, 1); // 從陣列中移除那個 member
    //         return [200, { ErrorCode: 1 }]
    //     }

    //     return [200, { ErrorCode: 12 }]
    // })
}