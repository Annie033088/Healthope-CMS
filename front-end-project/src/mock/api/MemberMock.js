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
            CreateTime: "2025-02-01T00:00:00",
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
            CreateTime: "2025-02-01T00:00:00",
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
            CreateTime: "2025-02-01T00:00:00",
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
            CreateTime: "2025-02-01T00:00:00",
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
            CreateTime: "2025-02-01T00:00:00",
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
            CreateTime: "2025-02-01T00:00:00",
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
            CreateTime: "2025-02-01T00:00:00",
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
            CreateTime: "2025-02-01T00:00:00",
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
            CreateTime: "2025-02-01T00:00:00",
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
            CreateTime: "2025-02-01T00:00:00",
        },
        {
            MemberId: 11,
            Name: "陳小明",
            Phone: 920222233,
            Email: "",
            BirthDay: "0001-01-01",
            Gender: 0,
            Height: 0,
            Weight: 0,
            Status: true,
            AbsenceTime: 3,
            AllowGroupClass: "2025-05-01T00:00:00",
            MembershipExpiry: "2025-11-01T00:00:00",
            PhoneVerified: false,
            EmergencyContactName: "",
            EmergencyContactPhone: 0,
            EmergencyContactRelation: "",
            CreateTime: "2025-01-06T00:00:00",
        },
        {
            MemberId: 12,
            Name: "陳千千",
            Phone: 920221233,
            Email: "",
            BirthDay: "0001-01-01",
            Gender: 0,
            Height: 0,
            Weight: 0,
            Status: true,
            AbsenceTime: 3,
            AllowGroupClass: "2025-05-01T00:00:00",
            MembershipExpiry: "2025-11-01T00:00:00",
            PhoneVerified: false,
            EmergencyContactName: "",
            EmergencyContactPhone: 0,
            EmergencyContactRelation: "",
            CreateTime: "2025-01-06T00:00:00",
        }
    ];

    const memberMembershipPlans = [
        {
            "MemberMembershipPlanId": 1,
            "OrderId": 4,
            "MemberId": 8,
            "PlanName": "一年會籍",
            "Duration": 12,
            "Status": 1,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-16 06:23:52.274",
            "UpdateTime": "2025-06-16 06:23:52.274"
        },
        {
            "MemberMembershipPlanId": 2,
            "OrderId": 5,
            "MemberId": 1,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 1,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-16 06:48:35.004",
            "UpdateTime": "2025-06-16 06:48:35.004"
        },
        {
            "MemberMembershipPlanId": 3,
            "OrderId": 6,
            "MemberId": 10,
            "PlanName": "一年會籍",
            "Duration": 12,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-16 07:00:15.739",
            "UpdateTime": "2025-06-16 07:00:15.739"
        },
        {
            "MemberMembershipPlanId": 4,
            "OrderId": 7,
            "MemberId": 6,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 1,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-16 07:02:05.876",
            "UpdateTime": "2025-06-16 07:02:05.876"
        },
        {
            "MemberMembershipPlanId": 5,
            "OrderId": 2,
            "MemberId": 3,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 1,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-17 08:10:51.121",
            "UpdateTime": "2025-06-17 08:10:51.121"
        },
        {
            "MemberMembershipPlanId": 7,
            "OrderId": 21,
            "MemberId": 2,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-20 01:41:52.149",
            "UpdateTime": "2025-06-20 01:41:52.149"
        },
        {
            "MemberMembershipPlanId": 8,
            "OrderId": 23,
            "MemberId": 11,
            "PlanName": "一年會籍",
            "Duration": 12,
            "Status": 1,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-20 01:56:35.795",
            "UpdateTime": "2025-06-20 01:56:35.795"
        },
        {
            "MemberMembershipPlanId": 9,
            "OrderId": 24,
            "MemberId": 1,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 1,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-20 02:03:14.480",
            "UpdateTime": "2025-06-20 02:03:14.480"
        },
        {
            "MemberMembershipPlanId": 10,
            "OrderId": 25,
            "MemberId": 12,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 1,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-20 02:12:42.028",
            "UpdateTime": "2025-06-20 02:12:42.028"
        },
        {
            "MemberMembershipPlanId": 11,
            "OrderId": 26,
            "MemberId": 4,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 1,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-20 02:15:07.469",
            "UpdateTime": "2025-06-20 02:15:07.469"
        },
        {
            "MemberMembershipPlanId": 12,
            "OrderId": 31,
            "MemberId": 6,
            "PlanName": "半年會籍",
            "Duration": 6,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-23 09:08:09.476",
            "UpdateTime": "2025-06-23 09:08:09.476"
        },
        {
            "MemberMembershipPlanId": 13,
            "OrderId": 32,
            "MemberId": 9,
            "PlanName": "半年會籍",
            "Duration": 6,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-23 09:39:31.146",
            "UpdateTime": "2025-06-23 09:39:31.146"
        },
        {
            "MemberMembershipPlanId": 14,
            "OrderId": 33,
            "MemberId": 2,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-24 07:08:23.095",
            "UpdateTime": "2025-06-24 07:08:23.095"
        },
        {
            "MemberMembershipPlanId": 15,
            "OrderId": 34,
            "MemberId": 10,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-24 07:08:59.773",
            "UpdateTime": "2025-06-24 07:08:59.773"
        },
        {
            "MemberMembershipPlanId": 16,
            "OrderId": 35,
            "MemberId": 7,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-24 08:00:05.123",
            "UpdateTime": "2025-06-24 08:00:05.123"
        },
        {
            "MemberMembershipPlanId": 17,
            "OrderId": 36,
            "MemberId": 5,
            "PlanName": "一年會籍",
            "Duration": 12,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-24 08:05:15.081",
            "UpdateTime": "2025-06-24 08:05:15.081"
        },
        {
            "MemberMembershipPlanId": 18,
            "OrderId": 38,
            "MemberId": 1,
            "PlanName": "半年會籍",
            "Duration": 6,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-24 08:45:18.012",
            "UpdateTime": "2025-06-24 08:45:18.012"
        },
        {
            "MemberMembershipPlanId": 19,
            "OrderId": 37,
            "MemberId": 11,
            "PlanName": "一年會籍",
            "Duration": 12,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-24 08:57:21.541",
            "UpdateTime": "2025-06-24 08:57:21.541"
        },
        {
            "MemberMembershipPlanId": 20,
            "OrderId": 39,
            "MemberId": 8,
            "PlanName": "半年會籍",
            "Duration": 6,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-24 09:16:07.656",
            "UpdateTime": "2025-06-24 09:16:07.656"
        },
        {
            "MemberMembershipPlanId": 21,
            "OrderId": 40,
            "MemberId": 3,
            "PlanName": "一年會籍",
            "Duration": 12,
            "Status": 1,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-24 09:50:35.265",
            "UpdateTime": "2025-06-24 09:50:35.265"
        },
        {
            "MemberMembershipPlanId": 22,
            "OrderId": 49,
            "MemberId": 6,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-30 02:02:40.849",
            "UpdateTime": "2025-06-30 02:02:40.849"
        },
        {
            "MemberMembershipPlanId": 23,
            "OrderId": 50,
            "MemberId": 9,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 3,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-06-30 02:38:54.136",
            "UpdateTime": "2025-06-30 02:38:54.136"
        },
        {
            "MemberMembershipPlanId": 24,
            "OrderId": 51,
            "MemberId": 2,
            "PlanName": "3 個月會籍",
            "Duration": 3,
            "Status": 1,
            "EndDate": "0001-01-01",
            "CreateTime": "2025-07-02 09:42:55.890",
            "UpdateTime": "2025-07-02 09:42:55.890"
        }
    ]

    const memberPersonalTrainingPackage = [
        {
            "MemberPersonalTrainingPackageId": 2,
            "OrderId": 9,
            "CoachId": 8,
            "MemberId": 5,
            "PlanName": "12 堂基本課",
            "UsedSessionCount": 0,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-16 07:24:11.301",
            "UpdateTime": "2025-06-16 07:24:11.301"
        },
        {
            "MemberPersonalTrainingPackageId": 3,
            "OrderId": 10,
            "CoachId": 8,
            "MemberId": 11,
            "PlanName": "12 堂基本課",
            "UsedSessionCount": 12,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-16 07:25:54.136",
            "UpdateTime": "2025-06-16 07:25:54.136"
        },
        {
            "MemberPersonalTrainingPackageId": 4,
            "OrderId": 16,
            "CoachId": 8,
            "MemberId": 3,
            "PlanName": "12 堂基本課",
            "UsedSessionCount": 5,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-16 08:25:48.539",
            "UpdateTime": "2025-06-16 08:25:48.539"
        },
        {
            "MemberPersonalTrainingPackageId": 5,
            "OrderId": 12,
            "CoachId": 8,
            "MemberId": 7,
            "PlanName": "12 堂基本課",
            "UsedSessionCount": 3,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-17 08:11:14.658",
            "UpdateTime": "2025-06-17 08:11:14.658"
        },
        {
            "MemberPersonalTrainingPackageId": 6,
            "OrderId": 22,
            "CoachId": 8,
            "MemberId": 1,
            "PlanName": "12 堂基本課",
            "UsedSessionCount": 10,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-20 01:50:05.010",
            "UpdateTime": "2025-06-20 01:50:05.010"
        },
        {
            "MemberPersonalTrainingPackageId": 7,
            "OrderId": 27,
            "CoachId": 8,
            "MemberId": 9,
            "PlanName": "12 堂基本課",
            "UsedSessionCount": 2,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-23 05:47:03.224",
            "UpdateTime": "2025-06-23 05:47:03.224"
        },
        {
            "MemberPersonalTrainingPackageId": 8,
            "OrderId": 15,
            "CoachId": 8,
            "MemberId": 12,
            "PlanName": "12 堂基本課",
            "UsedSessionCount": 12,
            "SessionCount": 12,
            "Status": 2,
            "CreateTime": "2025-06-24 09:00:25.063",
            "UpdateTime": "2025-06-24 09:00:25.063"
        },
        {
            "MemberPersonalTrainingPackageId": 9,
            "OrderId": 13,
            "CoachId": 8,
            "MemberId": 6,
            "PlanName": "12 堂基本課",
            "UsedSessionCount": 6,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-24 09:08:36.987",
            "UpdateTime": "2025-06-24 09:08:36.987"
        }
    ]

    const coaches = [
        {
            "CoachId": 1,
            "Name": "教練123",
            "Email": "教練123@example.com"
        },
        {
            "CoachId": 2,
            "Name": "教練456",
            "Email": "教練456@example.com"
        },
        {
            "CoachId": 3,
            "Name": "喵練",
            "Email": "qweqwpp22@.poi.pp"
        },
        {
            "CoachId": 4,
            "Name": "草莓123",
            "Email": "草莓123@example.com"
        },
        {
            "CoachId": 5,
            "Name": "草莓人456",
            "Email": "草莓人456@example.com"
        },
        {
            "CoachId": 6,
            "Name": "戚戚",
            "Email": "戚戚@example.com"
        },
        {
            "CoachId": 7,
            "Name": "Alice",
            "Email": "QWEGGG@WEB.33"
        },
        {
            "CoachId": 8,
            "Name": "Cathy",
            "Email": "ASDeath@gg.wc"
        },
        {
            "CoachId": 9,
            "Name": "Bob",
            "Email": "bob@example.com"
        },
        {
            "CoachId": 10,
            "Name": "ChaCha",
            "Email": "abcgg@ww.cc"
        },
        {
            "CoachId": 11,
            "Name": "Gellien",
            "Email": "gellien@example.com"
        },
        {
            "CoachId": 12,
            "Name": "qwewq",
            "Email": "qwewq@example.com"
        }
    ]

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
        let memberMembershipPlanTargets = memberMembershipPlans.filter(plan => plan.MemberId === Number(memberIdDto.MemberId))
        let memberPersonalTrainingPackageTargets = memberPersonalTrainingPackage.filter(plan => plan.MemberId === Number(memberIdDto.MemberId))

        const ApiDataObject = {
            Member: memberTarget, MemberMembershipPlanList: memberMembershipPlanTargets,
            MemberPersonalTrainingPackageList: memberPersonalTrainingPackageTargets,
            CoachList: coaches
        }

        if (memberTarget) {
            return [200, { ErrorCode: 1, ApiDataObject }]
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
            const matchPhone = !SearchPhone || item.Phone.toString().slice(-3) === SearchPhone;
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

    mock.onPost("/api/Member/GetMemberByNameOrPhone").reply(config => {
        let {
            Phone,
            Name,
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = members.filter(item => {
            const matchPhone = Phone === null || item.Phone === Number(Phone);
            const matchName = !Name || item.Name.includes(Name);
            return matchName && matchPhone;
        });

        return [200, { ErrorCode: 1, ApiDataObject: filtered }]
    })
}