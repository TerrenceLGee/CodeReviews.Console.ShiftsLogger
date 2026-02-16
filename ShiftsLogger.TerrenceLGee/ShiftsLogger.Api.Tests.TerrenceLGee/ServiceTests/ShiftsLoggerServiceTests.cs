using Moq;
using ShiftsLogger.Api.TerrenceLGee.Models;
using ShiftsLogger.Api.TerrenceLGee.Repositories.Interfaces;
using ShiftsLogger.Api.TerrenceLGee.Services;
using ShiftsLogger.Api.TerrenceLGee.Shared.Parameters;
using ShiftsLogger.Api.Tests.TerrenceLGee.Resources;

namespace ShiftsLogger.Api.Tests.TerrenceLGee.ServiceTests;

public class ShiftsLoggerServiceTests
{
    private readonly Mock<IShiftsLoggerRepository> _mockRepo;
    private readonly ShiftsLoggerService _shiftsLoggerService;

    public ShiftsLoggerServiceTests()
    {
        _mockRepo = new Mock<IShiftsLoggerRepository>();
        _shiftsLoggerService = new ShiftsLoggerService(_mockRepo.Object);
    }

    [Fact]
    public async Task AddShiftAsync_Returns_ResultWithRetrievedShiftDto_WhenSuccessful()
    {
        var expectedResult = ServiceResources.GetResultFromShiftAddSuccess();

        _mockRepo
            .Setup(r => r.AddShiftAsync(It.IsAny<Shift>()))
            .ReturnsAsync(RepositoryResources.GetShift());

        var actualResult = await _shiftsLoggerService.AddShiftAsync(ServiceResources.GetCreateShiftDtoBeforeAdd());

        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult.Value);
        Assert.NotNull(expectedResult.Value);
        Assert.Null(actualResult.ErrorMessage);
        Assert.True(actualResult.IsSuccess);
        Assert.False(actualResult.IsFailure);
        Assert.Equal(expectedResult.IsSuccess, actualResult.IsSuccess);
        Assert.Equal(expectedResult.Value.Id, actualResult.Value.Id);
        Assert.Equal(expectedResult.Value.UserId, actualResult.Value.UserId);
        Assert.Equal(expectedResult.Value.ShiftStart, actualResult.Value.ShiftStart);
        Assert.Equal(expectedResult.Value.ShiftEnd, actualResult.Value.ShiftEnd);
        Assert.Equal(expectedResult.Value.Duration, actualResult.Value.Duration);
    }

    [Fact]
    public async Task AddShiftAsync_Returns_ResultWithNull_WhenFailed()
    {
        var expectedResult = ServiceResources.GetResultFromShiftAddFailure();
        Shift? repoReturns = null;

        _mockRepo
            .Setup(r => r.AddShiftAsync(It.IsAny<Shift>()))
            .ReturnsAsync(repoReturns);

        var actualResult = await _shiftsLoggerService.AddShiftAsync(ServiceResources.GetCreateShiftDtoBeforeAdd());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.Null(expectedResult.Value);
        Assert.Null(actualResult.Value);
        Assert.False(actualResult.IsSuccess);
        Assert.True(actualResult.IsFailure);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.Contains("Error adding new shift", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task UpdateShiftAsync_Returns_ResultWithRetrievedShiftDto_WhenSuccessful()
    {
        var expectedResult = ServiceResources.GetResultFromShiftUpdateSuccess();

        _mockRepo
           .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
           .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.IsValidShiftOwnership(It.IsAny<UserParams>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.UpdateShiftAsync(It.IsAny<Shift>()))
            .ReturnsAsync(RepositoryResources.GetShiftAfterShiftUpdateSuccess());

        var actualResult = await _shiftsLoggerService.UpdateShiftAsync(ServiceResources.GetUpdateShiftDtoBeforeUpdate());

        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult.Value);
        Assert.NotNull(expectedResult.Value);
        Assert.Null(actualResult.ErrorMessage);
        Assert.True(actualResult.IsSuccess);
        Assert.False(actualResult.IsFailure);
        Assert.Equal(expectedResult.IsSuccess, actualResult.IsSuccess);
        Assert.Equal(expectedResult.Value.Id, actualResult.Value.Id);
        Assert.Equal(expectedResult.Value.UserId, actualResult.Value.UserId);
        Assert.Equal(expectedResult.Value.ShiftStart, actualResult.Value.ShiftStart);
        Assert.Equal(expectedResult.Value.ShiftEnd, actualResult.Value.ShiftEnd);
        Assert.Equal(expectedResult.Value.Duration, actualResult.Value.Duration);
    }

    [Fact]
    public async Task UpdateShiftAsync_Returns_ResultWithNull_WhenShiftNotFound()
    {
        var expectedResult = ServiceResources.GetResultFromShiftUpdateFailureWhenShiftNotFound();

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(false);

        var actualResult = await _shiftsLoggerService.UpdateShiftAsync(ServiceResources.GetUpdateShiftDtoBeforeUpdate());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.Null(expectedResult.Value);
        Assert.Null(actualResult.Value);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.IsFailure, actualResult.IsFailure);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.Contains("No shift with id 1 found", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task UpdateShiftAsync_Returns_ResultWithNull_WhenShiftDoesNotBelongToUser()
    {
        var expectedResult = ServiceResources.GetResultFromUpdateShiftFailureWhenShiftDoesNotBelongToUser();

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.IsValidShiftOwnership(It.IsAny<UserParams>()))
            .ReturnsAsync(false);

        var actualResult = await _shiftsLoggerService.UpdateShiftAsync(ServiceResources.GetUpdateShiftDtoBeforeUpdate());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.Null(expectedResult.Value);
        Assert.Null(actualResult.Value);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.IsFailure, actualResult.IsFailure);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.Contains("Invalid credentials for accessing or modifying shift 1", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task UpdateShiftAsync_Returns_ResultWithNull_WhenRepoReturnsNullOnUpdateFailure()
    {
        var expectedResult = ServiceResources.GetResultFromShiftUpdateFailureShiftIsNull();
        Shift? repoReturns = null;

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.IsValidShiftOwnership(It.IsAny<UserParams>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.UpdateShiftAsync(It.IsAny<Shift>()))
            .ReturnsAsync(repoReturns);

        var actualResult = await _shiftsLoggerService.UpdateShiftAsync(ServiceResources.GetUpdateShiftDtoBeforeUpdate());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.Null(expectedResult.Value);
        Assert.Null(actualResult.Value);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.IsFailure, actualResult.IsFailure);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.Contains("Error updating shift 1", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task DeleteShiftAsync_Returns_ResultSuccess_WhenDeletionSuccessful()
    {
        var expectedResult = ServiceResources.GetResultFromShiftDeletionSuccess();

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.IsValidShiftOwnership(It.IsAny<UserParams>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.DeleteShiftAsync(It.IsAny<UserParams>()))
            .ReturnsAsync(true);

        var actualResult = await _shiftsLoggerService.DeleteShiftAsync(ServiceResources.GetUserParams());

        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult);
        Assert.Equal(expectedResult.IsSuccess, actualResult.IsSuccess);
        Assert.Null(actualResult.ErrorMessage);
        Assert.True(actualResult.IsSuccess);
        Assert.False(actualResult.IsFailure);
    }

    [Fact]
    public async Task DeleteShiftAsync_Returns_ResultFailure_WhenShiftNotFound()
    {
        var expectedResult = ServiceResources.GetResultFromShiftDeleteFailureWhenShiftNotFound();

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(false);

        var actualResult = await _shiftsLoggerService.DeleteShiftAsync(ServiceResources.GetUserParams());

        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.IsFailure, actualResult.IsFailure);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.Contains("No shift with id 1 found", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task DeleteShiftAsync_Returns_ResultFailure_WhenShiftDoesNotBelongToUser()
    {
        var expectedResult = ServiceResources.GetResultFromDeleteShiftFailureWhenShiftDoesNotBelongToUser();

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.IsValidShiftOwnership(It.IsAny<UserParams>()))
            .ReturnsAsync(false);

        var actualResult = await _shiftsLoggerService.DeleteShiftAsync(ServiceResources.GetUserParams());

        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.IsFailure, actualResult.IsFailure);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.Contains("Invalid credentials for accessing or modifying shift 1", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task DeleteShiftAsync_Returns_ResultFailure_WhenRepoReturnsFalseOnDeleteFailure()
    {
        var expectedResult = ServiceResources.GetResultFromShiftDeletionFailure();

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.IsValidShiftOwnership(It.IsAny<UserParams>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.DeleteShiftAsync(It.IsAny<UserParams>()))
            .ReturnsAsync(false);

        var actualResult = await _shiftsLoggerService.DeleteShiftAsync(ServiceResources.GetUserParams());

        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.IsFailure, actualResult.IsFailure);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.Contains("Error deleting shift 1", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task GetShiftAsync_ReturnsResultWithRetrievedShiftDto_WhenShiftRetrievalSucceeds()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftSuccess();

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.IsValidShiftOwnership(It.IsAny<UserParams>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.GetShiftAsync(It.IsAny<UserParams>()))
            .ReturnsAsync(RepositoryResources.GetShift());

        var actualResult = await _shiftsLoggerService.GetShiftAsync(ServiceResources.GetUserParams());

        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult.Value);
        Assert.NotNull(expectedResult.Value);
        Assert.Null(actualResult.ErrorMessage);
        Assert.True(actualResult.IsSuccess);
        Assert.False(actualResult.IsFailure);
        Assert.Equal(expectedResult.IsSuccess, actualResult.IsSuccess);
        Assert.Equal(expectedResult.Value.Id, actualResult.Value.Id);
        Assert.Equal(expectedResult.Value.UserId, actualResult.Value.UserId);
        Assert.Equal(expectedResult.Value.ShiftStart, actualResult.Value.ShiftStart);
        Assert.Equal(expectedResult.Value.ShiftEnd, actualResult.Value.ShiftEnd);
        Assert.Equal(expectedResult.Value.Duration, actualResult.Value.Duration);
    }

    [Fact]
    public async Task GetShiftAsync_ReturnsResultWithNull_WhenShiftNotFound()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftFailureWhenShiftNotFound();

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(false);

        var actualResult = await _shiftsLoggerService.GetShiftAsync(ServiceResources.GetUserParams());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.Null(expectedResult.Value);
        Assert.Null(actualResult.Value);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.IsFailure, actualResult.IsFailure);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.Contains("No shift with id 1 found", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task DeleteShiftAsync_Returns_ResultWithNull_WhenShiftDoesNotBelongToUser()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftFailureWhenShiftDoesNotBelongToUser();

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.IsValidShiftOwnership(It.IsAny<UserParams>()))
            .ReturnsAsync(false);

        var actualResult = await _shiftsLoggerService.GetShiftAsync(ServiceResources.GetUserParams());

        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.IsFailure, actualResult.IsFailure);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.Contains("Invalid credentials for accessing or modifying shift 1", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task DeleteShiftAsync_Returns_ResultWithNull_WhenRepoReturnsNullOnRetrievalFailure()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftFailure();
        Shift? repoReturns = null;

        _mockRepo
            .Setup(r => r.IsValidShiftId(It.IsAny<int>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.IsValidShiftOwnership(It.IsAny<UserParams>()))
            .ReturnsAsync(true);

        _mockRepo
            .Setup(r => r.GetShiftAsync(It.IsAny<UserParams>()))
            .ReturnsAsync(repoReturns);

        var actualResult = await _shiftsLoggerService.GetShiftAsync(ServiceResources.GetUserParams());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.Null(expectedResult.Value);
        Assert.Null(actualResult.Value);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.IsFailure, actualResult.IsFailure);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.Contains("Error retrieving shift 1", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task GetShiftsAsync_ReturnsResultWithPagedListOfRetrievedShiftDto_OnSuccess()
    {
        var expectedResult = ServiceResources.GetResultFromGetShifts();

        _mockRepo
            .Setup(r => r.GetShiftsAsync(It.IsAny<PaginationParams>()))
            .ReturnsAsync(RepositoryResources.GetPagedListOfShifts(RepositoryResources.GetPaginationParams()));

        var actualResult = await _shiftsLoggerService.GetShiftsAsync(ServiceResources.GetPaginationParams());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult.Value);
        Assert.NotNull(actualResult.Value);
        Assert.NotEmpty(actualResult.Value);
        Assert.Equal(1, actualResult.Value.PageNumber);
        Assert.Equal(3, actualResult.Value.TotalPages);
    }

    [Fact]
    public async Task GetShiftsAsync_ReturnsResultWithEmptyPagedListOfResultRetrievedShiftDto_WhenRepoCatchesException()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftsWhenRepoCatchesException();

        _mockRepo
            .Setup(r => r.GetShiftsAsync(It.IsAny<PaginationParams>()))
            .ReturnsAsync(RepositoryResources.GetShiftsWhenRepoCatchesException(RepositoryResources.GetPaginationParams()));

        var actualResult = await _shiftsLoggerService.GetShiftsAsync(ServiceResources.GetPaginationParams());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult.Value);
        Assert.NotNull(actualResult.Value);
        Assert.Empty(actualResult.Value);
        Assert.Equal(0, actualResult.Value.TotalPages);
    }

    [Fact]
    public async Task GetAllShiftsForAdminAsync_ReturnsResultWithPagedListOfRetrievedShiftDto_OnSuccess()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftsForAdmin();

        _mockRepo
            .Setup(r => r.GetAllShiftsForAdminAsync(It.IsAny<PaginationParams>()))
            .ReturnsAsync(RepositoryResources.GetPagedListOfShiftsForAdmin(RepositoryResources.GetPaginationParams()));

        var actualResult = await _shiftsLoggerService.GetAllShiftsForAdminAsync(ServiceResources.GetPaginationParams());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult.Value);
        Assert.NotNull(actualResult.Value);
        Assert.NotEmpty(actualResult.Value);
        Assert.Equal(1, actualResult.Value.PageNumber);
        Assert.Equal(3, actualResult.Value.TotalPages);
    }

    [Fact]
    public async Task GetAllShiftsForAdminAsync_ReturnsResultWithEmptyPagedListOfResultRetrievedShiftDto_WhenRepoCatchesException()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftsForAdminWhenRepoCatchesException();

        _mockRepo
            .Setup(r => r.GetAllShiftsForAdminAsync(It.IsAny<PaginationParams>()))
            .ReturnsAsync(RepositoryResources.GetShiftsForAdminWhenRepoCatchesException(RepositoryResources.GetPaginationParams()));

        var actualResult = await _shiftsLoggerService.GetAllShiftsForAdminAsync(ServiceResources.GetPaginationParams());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.NotNull(expectedResult.Value);
        Assert.NotNull(actualResult.Value);
        Assert.Empty(actualResult.Value);
        Assert.Equal(0, actualResult.Value.TotalPages);
    }

    [Fact]
    public async Task GetCountOfShiftsAsync_ReturnsResultWithCountOfShiftsForUser_WhenSuccessful()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftCountWhenThereAreShifts();

        _mockRepo
            .Setup(r => r.GetCountOfShiftsAsync(It.IsAny<UserParams>()))
            .ReturnsAsync(RepositoryResources.ShiftCount);

        var actualResult = await _shiftsLoggerService.GetCountOfShiftsAsync(ServiceResources.GetUserParams());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.Null(actualResult.ErrorMessage);
        Assert.True(actualResult.IsSuccess);
        Assert.False(actualResult.IsFailure);
        Assert.Equal(expectedResult.Value, actualResult.Value);
        Assert.Equal(ServiceResources.ShiftCount, actualResult.Value);
    }

    [Fact]
    public async Task GetCountOfShiftsAsync_ReturnsResultFailure_WhenRepoCatchesException()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftCountWhenRepoCatchesException();

        _mockRepo
            .Setup(r => r.GetCountOfShiftsAsync(It.IsAny<UserParams>()))
            .ReturnsAsync(RepositoryResources.WhenRepoCatchesExceptionDuringCount);

        var actualResult = await _shiftsLoggerService.GetCountOfShiftsAsync(ServiceResources.GetUserParams());

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.Value, actualResult.Value);
        Assert.Contains("Error retrieving the count of shifts", actualResult.ErrorMessage);
    }

    [Fact]
    public async Task GetCountOfAllShiftsForAdminAsync_ReturnsResultWithCountOfShiftsForUser_WhenSuccessful()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftCountForAdminWhenThereAreShifts();

        _mockRepo
            .Setup(r => r.GetCountOfAllShiftsForAdminAsync())
            .ReturnsAsync(RepositoryResources.ShiftCount);

        var actualResult = await _shiftsLoggerService.GetCountOfAllShiftsForAdminAsync();

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.Null(actualResult.ErrorMessage);
        Assert.True(actualResult.IsSuccess);
        Assert.False(actualResult.IsFailure);
        Assert.Equal(expectedResult.Value, actualResult.Value);
        Assert.Equal(ServiceResources.ShiftCount, actualResult.Value);
    }

    [Fact]
    public async Task GetCountOfAllShiftsForAdminAsync_ReturnsResultFailure_WhenRepoCatchesException()
    {
        var expectedResult = ServiceResources.GetResultFromGetShiftCountForAdminWhenRepoCatchesException();

        _mockRepo
            .Setup(r => r.GetCountOfAllShiftsForAdminAsync())
            .ReturnsAsync(RepositoryResources.WhenRepoCatchesExceptionDuringCount);

        var actualResult = await _shiftsLoggerService.GetCountOfAllShiftsForAdminAsync();

        Assert.NotNull(expectedResult);
        Assert.NotNull(actualResult);
        Assert.NotNull(actualResult.ErrorMessage);
        Assert.True(actualResult.IsFailure);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.Value, actualResult.Value);
        Assert.Contains("Error retrieving the count of all shifts", actualResult.ErrorMessage);
    }
}
