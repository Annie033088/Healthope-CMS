
export default function (mock) {
    mock.onPost("/api/MemberPlan/EditMemberMembershipPlanStatus").reply(() => {
        return [200, { ErrorCode: 1 }]
    }),
    mock.onPost("/api/MemberPlan/EditMemberPersonalTrainingPackageCoach").reply(() => {
        return [200, { ErrorCode: 1 }]
    })
}