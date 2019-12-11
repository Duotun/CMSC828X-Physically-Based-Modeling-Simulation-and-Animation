
K = readmatrix("Kmatrix.csv");
M = readmatrix("Mmatrix.csv");
tic;
[V,D] = eig(K,M);
%utilize matlab here to process G and D - mode compression
% 1 - 2 - 22000 range of requency
% D value -> sqrt(2pi * frequency)
x=[];
%A= magic(4)
%A(x,:)=[];
%A(:,x)=[];
for i = 1: size(D,1)
        if (2*pi*2).^2 >D(i,i) | (2*pi*20000).^2 < D(i,i)
            x=[x,i];
        end
end
V(:,x)=[];
D(x,:)=[];
D(:,x)=[];

%mode Truncation
k1 = 1/1750;
b1=6/7;
k2=14*10e-3;
b2=-26;
x=[];
for i =1:size(D)
    if (i+1)<=size(D)
        if D(i,i) <= (2*pi*2000).^2
            if abs((sqrtpi(D(i,i))-sqrtpi(D(i+1,i+1))))<= (k1*sqrtpi(D(i,i))+b1)
                %merge
                tmp = V(:,i)+V(:,i+1);
                V(:,i)=tmp;
                x=[x,i+1];
            end
        else
             if abs((sqrtpi(D(i,i))-sqrtpi(D(i+1,i+1))))<= (k2*sqrtpi(D(i,i))+b2)
                %merge
                tmp = V(:,i)+V(:,i+1);
                V(:,i)=tmp;
                x=[x,i+1];
             end
        end
    end
end
     
V(:,x)=[];
D(x,:)=[];
D(:,x)=[];


toc;
writematrix(V,"Gmatrix.csv");
writematrix(D,"Dmatrix.csv");

function y = sqrtpi(x)    % y ->return value, x - input value
        y = 1/(2*pi)*sqrt(x);
end


