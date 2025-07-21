export default function (mock) {
    const memberPersonalTrainingPackage = [
        {
            "MemberPersonalTrainingPackageId": 2,
            "OrderId": 9,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 5,
            "MemberName": "林阿忠",
            "MemberPhone": 915667788,
            "PlanName": "12 堂基本課",
            "UsedSession": 5,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-16 07:24:11.301",
            "UpdateTime": "2025-06-16 07:24:11.301"
        },
        {
            "MemberPersonalTrainingPackageId": 3,
            "OrderId": 10,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 11,
            "MemberName": "陳小明",
            "MemberPhone": 920222233,
            "PlanName": "12 堂基本課",
            "UsedSession": 5,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-16 07:25:54.136",
            "UpdateTime": "2025-06-16 07:25:54.136"
        },
        {
            "MemberPersonalTrainingPackageId": 4,
            "OrderId": 16,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 3,
            "MemberName": "李大仁",
            "MemberPhone": 913334455,
            "PlanName": "12 堂基本課",
            "UsedSession": 8,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-16 08:25:48.539",
            "UpdateTime": "2025-06-16 08:25:48.539"
        },
        {
            "MemberPersonalTrainingPackageId": 5,
            "OrderId": 12,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 7,
            "MemberName": "鄭家豪",
            "MemberPhone": 917889900,
            "PlanName": "12 堂基本課",
            "UsedSession": 0,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-17 08:11:14.658",
            "UpdateTime": "2025-06-17 08:11:14.658"
        },
        {
            "MemberPersonalTrainingPackageId": 6,
            "OrderId": 22,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 1,
            "MemberName": "王小明",
            "MemberPhone": 912345678,
            "PlanName": "12 堂基本課",
            "UsedSession": 1,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-20 01:50:05.010",
            "UpdateTime": "2025-06-20 01:50:05.010"
        },
        {
            "MemberPersonalTrainingPackageId": 7,
            "OrderId": 27,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 9,
            "MemberName": "曾明志",
            "MemberPhone": 919101112,
            "PlanName": "12 堂基本課",
            "UsedSession": 3,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-23 05:47:03.224",
            "UpdateTime": "2025-06-23 05:47:03.224"
        },
        {
            "MemberPersonalTrainingPackageId": 8,
            "OrderId": 15,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 12,
            "MemberName": "陳千千",
            "MemberPhone": 920221233,
            "PlanName": "12 堂基本課",
            "UsedSession": 3,
            "SessionCount": 12,
            "Status": 2,
            "CreateTime": "2025-06-24 09:00:25.063",
            "UpdateTime": "2025-06-24 09:00:25.063"
        },
        {
            "MemberPersonalTrainingPackageId": 9,
            "OrderId": 13,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 6,
            "MemberName": "周玉芬",
            "MemberPhone": 916778899,
            "PlanName": "12 堂基本課",
            "UsedSession": 6,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-24 09:08:36.987",
            "UpdateTime": "2025-06-24 09:08:36.987"
        }
    ]

    const memberPersonalClass = [
        {
            "MemberPersonalClassId": 2,
            "OrderId": 9,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 5,
            "MemberName": "林阿忠",
            "MemberPhone": 915667788,
            Category: 1,
            Remark: "",
            "Status": 1,
            "Time": "2025-07-16T07:24:11.301",
            "UpdateTime": "2025-06-16 07:24:11.301"
        },
        {
            "MemberPersonalClassId": 3,
            "OrderId": 10,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 11,
            "MemberName": "陳小明",
            "MemberPhone": 920222233,
            Category: 1,
            Remark: "",
            "Status": 1,
            "Time": "2025-07-20T09:24:11.301",
            "UpdateTime": "2025-06-16 07:25:54.136"
        },
        {
            "MemberPersonalClassId": 4,
            "OrderId": 16,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 3,
            "MemberName": "李大仁",
            "MemberPhone": 913334455,
            Category: 1,
            Remark: "",
            "Status": 1,
            "Time": "2025-07-01T13:24:11.301",
            "UpdateTime": "2025-06-16 08:25:48.539"
        },
        {
            "MemberPersonalClassId": 5,
            "OrderId": 12,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 7,
            "MemberName": "鄭家豪",
            "MemberPhone": 917889900,
            Category: 1,
            Remark: "",
            "Status": 1,
            "Time": "2025-06-30T14:24:11.301",
            "UpdateTime": "2025-06-17 08:11:14.658"
        },
        {
            "MemberPersonalClassId": 6,
            "OrderId": 22,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 1,
            "MemberName": "王小明",
            "MemberPhone": 912345678,
            Category: 1,
            Remark: "",
            "Status": 1,
            "Time": "2025-06-25T11:24:11.301",
            "UpdateTime": "2025-06-20 01:50:05.010"
        },
        {
            "MemberPersonalClassId": 7,
            "OrderId": 27,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 9,
            "MemberName": "曾明志",
            "MemberPhone": 919101112,
            Category: 1,
            "Status": 1,
            Remark: "",
            "Time": "2025-07-02T16:24:11.301",
            "UpdateTime": "2025-06-23 05:47:03.224"
        },
        {
            "MemberPersonalClassId": 8,
            "OrderId": 15,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 12,
            "MemberName": "陳千千",
            "MemberPhone": 920221233,
            Category: 1,
            "Status": 2,
            Remark: "",
            "Time": "2025-07-02T14:24:11.301",
            "UpdateTime": "2025-06-24 09:00:25.063"
        },
        {
            "MemberPersonalClassId": 9,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 6,
            "MemberName": "周玉芬",
            "MemberPhone": 916778899,
            Category: 1,
            "Status": 1,
            Remark: "",
            "Time": "2025-07-10T10:24:11.301",
            "UpdateTime": "2025-06-24 09:08:36.987"
        }
    ]


    mock.onPost("/api/MemberClass/GetPersonalTrainingPackageAndCoach").reply(config => {
        let {
            MemberId,
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = memberPersonalTrainingPackage.filter(item => {
            return MemberId === item.MemberId;
        });

        return [200, { ErrorCode: 1, ApiDataObject: filtered }]
    })

    mock.onPost("/api/MemberClass/AddMemberPersonalClass").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/MemberClass/GetMemberPersonalClass").reply(config => {
        let {
            Status,
            SortOrder,
            SortOption,
            RecordPerPage,
            SearchPhone,
            Page
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = memberPersonalClass.filter(item => {
            const matchStatus = Status === null || item.Status === Status;
            const matchPhone = !SearchPhone || item.MemberPhone.toString().slice(-3) === SearchPhone;
            return matchStatus && matchPhone;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "time") {
            field = "Time";
        } else if (SortOption === "coachId") {
            field = "CoachId";
        } else {
            field = "MemberPersonalClassId"
        }

        filtered.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (aVal < bVal) return SortOrder === 'descending' ? 1 : -1;
            if (aVal > bVal) return SortOrder === 'descending' ? -1 : 1;
            return 0;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const data = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            MemberPersonalClassList: data,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    mock.onPost("/api/MemberClass/EditMemberPersonalClassRemark").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/MemberClass/EditMemberPersonalClassStatus").reply(() => {
        return [200, { ErrorCode: 1 }]
    })
}
